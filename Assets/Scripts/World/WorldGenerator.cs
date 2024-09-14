using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    private TileNodePrefab[] tilePrefabs;

    public static readonly int worldSize = 11;
    public static readonly int tileScale = 20;


    private TileNode[,] worldTiles;

    private void Start()
    {
        tilePrefabs = Resources.LoadAll<TileNodePrefab>("Tiles");

        generateWorld();
    }

    private void generateWorld()
    {
        worldTiles = new TileNode[worldSize, worldSize];

        Vector3Int entrancePos = new Vector3Int(0, 0, worldSize / 2);

        worldTiles[entrancePos.x, entrancePos.z] = new TileNode(tilePrefabs[1].transform,entrancePos, Quaternion.identity, transform, tilePrefabs[1].connectingSides);

        StartCoroutine(generateAdjacentTiles(entrancePos.x, entrancePos.z));
    }

    private void generateTile(int x, int z)
    {
        TileNode[] adjacentTiles = getAdjacentTiles(x, z);
        Debug.Log(x + "|" + z + ": " + string.Join(", ", adjacentTiles.Select(item => item != null ? item.ToString() : "null")));

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

        Debug.Log(System.Convert.ToString(requiredConnections ^ existingAjdacentTiles, 2));

        //if (requiredConnections == 0 || existingAjdacentTiles == 0)
        //{
        //    worldTiles[x, z] = new TileNode(new Vector3Int(x, 0, z));
        //    return;
        //}

        List<(int, int)> compatibleTiles = getCompatibleTiles(requiredConnections, requiredConnections ^ existingAjdacentTiles);

        if (compatibleTiles.Count == 0)
        {
            worldTiles[x, z] = new TileNode(new Vector3Int(x, 0, z));
            return;
        }

        Debug.Log(string.Join(", ", compatibleTiles.Select(item => System.Convert.ToString(rotateRight(tilePrefabs[item.Item1].connectingSides, item.Item2), 2))));

        (int, int) indexAndRotation = compatibleTiles[Random.Range(0, compatibleTiles.Count)];

        TileNodePrefab prefab = tilePrefabs[indexAndRotation.Item1];

        worldTiles[x, z] = new TileNode(prefab.transform, new Vector3Int(x, 0, z), Quaternion.Euler(Vector3.up * 90 * indexAndRotation.Item2), transform, rotateRight(prefab.connectingSides, indexAndRotation.Item2));
    }


    private IEnumerator generateAdjacentTiles(int x, int z)
    {
        //yield return new WaitForSeconds(Random.Range(0f, 0.1f));
        yield return 0;

        TileNode[] adjacentTiles = getAdjacentTiles(x, z);

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
    private TileNode[] getAdjacentTiles(int x, int z)
    {
        TileNode[] tiles = new TileNode[4];

        for(int i = 0; i < 4; i++)
        {
            (int, int) pos = getSidePos(i, x, z);

            if (pos.Item1 < 0 || pos.Item1 >= worldSize || pos.Item2 < 0 || pos.Item2 >= worldSize)
            {
                tiles[i] = new TileNode(new Vector3Int(pos.Item1, 0, pos.Item2));
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

    private bool checkTileForConnections(TileNode tile, int side)
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

    public int rotateRight(int value, int rotations)
    {
        rotations = rotations % 4;

        for (int i = 0; i < rotations; i++)
        {
            value = (value >> 1) | ((value & 1) << 3);
        }

        return value;
    }

}
