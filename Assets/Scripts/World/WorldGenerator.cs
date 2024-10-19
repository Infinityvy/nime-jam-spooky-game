using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public static WorldGenerator instance;

    public event System.Action onFinishedGenerating;
    public bool tilesGenerated = false;

    [SerializeField]
    private NavMeshSurface hunterNavMesh;

    private TilePrefab[] tilePrefabs;
    private TilePrefab emptyTilePrefab;
    private TilePrefab startTilePrefab;

    private Dictionary<string, Transform> resourcePrefabs = new Dictionary<string, Transform>();

    public static readonly int worldSize = 9;
    public static readonly int tileScale = 10;

    private static int resourceAmount = 20;


    private Tile[,] worldTiles;
    private int generatedTileCount = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        tilePrefabs = Resources.LoadAll<TilePrefab>("Tiles");
        emptyTilePrefab = Resources.Load<TilePrefab>("SpecialTiles/tile_empty");
        startTilePrefab = Resources.Load<TilePrefab>("SpecialTiles/tile_elevator");

        Transform[] resourcePrefabsTmp = Resources.LoadAll<Transform>("ResourceNodes");
        foreach (Transform prefab in resourcePrefabsTmp)
        {
            resourcePrefabs.Add(prefab.name, prefab);
        }

        generateWorld();

        PlayerMovement.instance.transform.position = new Vector3Int(0, 0, worldSize / 2) * tileScale;
    }

    private void generateWorld()
    {
        worldTiles = new Tile[worldSize, worldSize];

        Vector3Int entrancePos = new Vector3Int(0, 0, worldSize / 2);

        worldTiles[entrancePos.x, entrancePos.z] = new Tile(startTilePrefab.transform, entrancePos, Quaternion.identity, transform, startTilePrefab.connectingSides);

        generatedTileCount = 1;

        StartCoroutine(generateAdjacentTiles(entrancePos.x, entrancePos.z));
    }

    private void generateTile(int x, int z)
    {
        generatedTileCount++;
        if (generatedTileCount == worldSize * worldSize && !tilesGenerated)
        {
            tilesGenerated = true;
            Invoke(nameof(generateResources), 0);
        }

        Tile[] adjacentTiles = getAdjacentTiles(x, z);
        //Debug.Log(x + "|" + z + ": " + string.Join(", ", adjacentTiles.Select(item => item != null ? item.ToString() : "null")));

        int requiredConnections = 0b0000;
        int existingAjdacentTiles = 0b0000;

        for(int i = 0; i < adjacentTiles.Length; i++)
        {
            int side = 0b1000 >> i;

            if (adjacentTiles[i] == null) continue;

            existingAjdacentTiles |= side;
            
            if(checkTileForConnections(adjacentTiles[i], getOppositeSide(side)))
            {
                requiredConnections |= side;
            }
        }

        //Debug.Log(System.Convert.ToString(requiredConnections ^ existingAjdacentTiles, 2));


        List<(int, int)> compatibleTiles = getCompatibleTiles(requiredConnections, requiredConnections ^ existingAjdacentTiles);

        if (compatibleTiles.Count == 0)
        {
            worldTiles[x, z] = new Tile(emptyTilePrefab.transform, new Vector3Int(x, 0, z), Quaternion.identity, transform);
            return;
        }

        //Debug.Log(string.Join(", ", compatibleTiles.Select(item => System.Convert.ToString(rotateRight(tilePrefabs[item.Item1].connectingSides, item.Item2), 2))));

        (int, int) indexAndRotation = compatibleTiles[Random.Range(0, compatibleTiles.Count)];

        TilePrefab prefab = tilePrefabs[indexAndRotation.Item1];

        worldTiles[x, z] = new Tile(prefab.transform, new Vector3Int(x, 0, z), Quaternion.Euler(Vector3.up * 90 * indexAndRotation.Item2), transform, rotateRight(prefab.connectingSides, indexAndRotation.Item2));
    }


    private IEnumerator generateAdjacentTiles(int x, int z)
    {
        yield return 0;

        Tile[] adjacentTiles = getAdjacentTiles(x, z);

        for (int i = 0; i < adjacentTiles.Length; ++i)
        {
            if(adjacentTiles[i] != null) continue;

            (int, int) pos = getSidePos(i, x, z);

            generateTile(pos.Item1, pos.Item2);

            StartCoroutine(generateAdjacentTiles(pos.Item1, pos.Item2));
        }
    }

    /// <summary>
    /// Sorted in order: top, right, bottom, left. TileNode will be null if no tile exists in that place.
    /// </summary>
    private Tile[] getAdjacentTiles(int x, int z)
    {
        Tile[] tiles = new Tile[4];

        for(int i = 0; i < 4; i++)
        {
            (int, int) pos = getSidePos(i, x, z);

            if (pos.Item1 < 0 || pos.Item1 >= worldSize || pos.Item2 < 0 || pos.Item2 >= worldSize)
            {
                tiles[i] = new Tile(emptyTilePrefab.transform, new Vector3Int(pos.Item1, 0, pos.Item2), Quaternion.identity, transform);
            }
            else
            {
                tiles[i] = worldTiles[pos.Item1, pos.Item2];
            }
        }

        return tiles;
    }

    private (int, int) getSidePos(int sideIndex, int x, int z)
    {
        (int, int) pos;

        switch (sideIndex)
        {
            case 0:
                pos = (x, z + 1);
                break;
            case 1:
                pos = (x + 1, z);
                break;
            case 2:
                pos = (x, z - 1);
                break;
            case 3:
                pos = (x - 1, z);
                break;
            default:
                throw new System.ArgumentException("Unkwon side index.");
        }

        return pos;
    }

    private bool checkTileForConnections(Tile tile, int side)
    {
        bool result = (tile.connectingSides & side) == side;

        //Debug.Log("Comparing " + System.Convert.ToString(tile.connectingSides, 2) + " with " + System.Convert.ToString(side, 2) + " resulting in " + result.ToString());

        return result;
    }
    
    /// <summary>
    /// Only supports a single side being set.
    /// </summary>
    private int getOppositeSide(int side)
    {
        switch (side)
        {
            case 0b1000: // Top
                return 0b0010; // Bottom
            case 0b0100: // Right
                return 0b0001; // Left
            case 0b0010: // Bottom
                return 0b1000; // Top
            case 0b0001: // Left
                return 0b0100; // Right
            default:
                throw new System.ArgumentException("Unkown side index.");
        }
    }

    private List<(int, int)> getCompatibleTiles(int requiredConnections, int forbiddenConnection)
    {
        List<(int, int)> compatibleTiles = new List<(int, int)>();

        for(int i = 0; i < tilePrefabs.Length; i++)
        {
            int connectingSides = tilePrefabs[i].connectingSides;

            for (int rotation = 0; rotation < 4; rotation++)
            {
                int rotatedSides = rotateRight(connectingSides, rotation);

                if (((rotatedSides & requiredConnections) == requiredConnections) &&
                    ((rotatedSides & forbiddenConnection) == 0))
                {
                    compatibleTiles.Add((i, rotation));
                }
            }
        }

        return compatibleTiles;
    }

    private int rotateRight(int value, int rotations)
    {
        rotations = rotations % 4;

        for (int i = 0; i < rotations; i++)
        {
            value = (value >> 1) | ((value & 1) << 3);
        }

        return value;
    }

    private void generateResources()
    {
        for (int i = 0; i < resourceAmount; i++)
        {
            Vector3 nodePosition = getRandomSpawnPosition();

            Instantiate(resourcePrefabs["resource_iron"], nodePosition, Quaternion.identity, transform);
        }

        hunterNavMesh.BuildNavMesh();

        onFinishedGenerating.Invoke();
    }

    public Vector3 getRandomSpawnPosition()
    {
        Tile tile = getRandomTile();

        return tile.getRandomSpawnPosition();
    }

    /// <summary>
    /// Only returns tiles that allow spawning objects.
    /// </summary>
    /// <returns></returns>
    public Tile getRandomTile()
    {
        Tile tile = null;

        while (tile == null || tile.nodeZones == null)
        {
            int x = Random.Range(0, worldSize);
            int z = Random.Range(0, worldSize);

            tile = worldTiles[x, z];
        }

        return tile;
    }

    public Tile getTile(int x, int z)
    {
        return worldTiles[x, z]; 
    }

    private void OnDrawGizmosSelected()
    {
        if(!Application.isPlaying || !tilesGenerated) return;

        Gizmos.color = Color.green;

        foreach(Tile tile in worldTiles)
        {
            if(tile.nodeZones == null) continue;
            foreach(Zone zone in tile.nodeZones)
            {
                (Vector3, Vector3) bounds = zone.getBounds();

                Vector3 corner1 = bounds.Item1;
                Vector3 corner2 = bounds.Item2;

                // Calculate the min and max for x and z
                float minX = Mathf.Min(corner1.x, corner2.x);
                float maxX = Mathf.Max(corner1.x, corner2.x);
                float minZ = Mathf.Min(corner1.z, corner2.z);
                float maxZ = Mathf.Max(corner1.z, corner2.z);

                // Get the 4 corner positions
                Vector3 bottomLeft = new Vector3(minX, 0, minZ) + tile.tileObject.position;
                Vector3 bottomRight = new Vector3(maxX, 0, minZ) + tile.tileObject.position;
                Vector3 topLeft = new Vector3(minX, 0, maxZ) + tile.tileObject.position;
                Vector3 topRight = new Vector3(maxX, 0, maxZ) + tile.tileObject.position;


                Gizmos.DrawLine(bottomLeft, bottomRight);
                Gizmos.DrawLine(bottomLeft, topLeft);
                Gizmos.DrawLine(topRight, bottomRight);
                Gizmos.DrawLine(topRight, topLeft);
            }
        }
    }
}
