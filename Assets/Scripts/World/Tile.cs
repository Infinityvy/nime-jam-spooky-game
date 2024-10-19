using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile
{
    // top, right, bottom, left
    public int connectingSides = 0b0000;

    public Transform tileObject = null;

    public Vector3Int tilePos = Vector3Int.zero;

    public Zone[] nodeZones = null;


    public Tile(Transform prefab, Vector3Int tilePos, Quaternion rotation, Transform parent, int connectingSides = 0b0000)
    {
        this.tilePos = tilePos;
        this.connectingSides = connectingSides;

        tileObject = GameObject.Instantiate(prefab, tilePos * WorldGenerator.tileScale, rotation, parent);
        GameObject.Destroy(tileObject.GetComponent<TilePrefab>());

        tileObject.name += " " + ToString() + " " + tilePos.ToString();

        nodeZones = tileObject.GetComponent<TilePrefab>().nodeZones;

        for (int i = 0; i < nodeZones.Length; i++)
        {
            nodeZones[i].rotate(rotation);
        }

        if (nodeZones != null && nodeZones.Length == 0) nodeZones = null;
    }

    public Vector3 getRandomSpawnPosition()
    {
        return nodeZones[Random.Range(0, nodeZones.Length)].getRandomPositionInZone() + tileObject.position;
    }

    public override string ToString()
    {
        return System.Convert.ToString(connectingSides, 2);
    }
}
