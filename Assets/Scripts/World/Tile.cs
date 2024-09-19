using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Tile(Vector3Int tilePos, Transform parent) 
    {
        connectingSides = 0b0000;
        this.tilePos = tilePos;

        tileObject = GameObject.Instantiate(Resources.Load<Transform>("EndTiles/tile_empty"), tilePos * WorldGenerator.tileScale, Quaternion.identity, parent);
        GameObject.Destroy(tileObject.GetComponent<TilePrefab>());

        tileObject.name += " " + Convert.ToString(connectingSides, 2) + " " + tilePos.ToString();
    }

    public Tile(Transform prefab, Vector3Int tilePos, Quaternion rotation, Transform parent, int connectingSides)
    {
        this.tilePos = tilePos;
        this.connectingSides = connectingSides;
        this.nodes = prefab.GetComponent<TilePrefab>().nodes;

        tileObject = GameObject.Instantiate(prefab, tilePos * WorldGenerator.tileScale, rotation, parent);
        GameObject.Destroy(tileObject.GetComponent<TilePrefab>());

        tileObject.name += " " + Convert.ToString(connectingSides, 2) + " " + tilePos.ToString();

        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = tileObject.rotation * nodes[i];
        }
    }

    // top, right, bottom, left
    public int connectingSides = 0b0000;

    public Transform tileObject = null;

    public Vector3Int tilePos = Vector3Int.zero;

    public Vector3[] nodes;

    public override string ToString()
    {
        return System.Convert.ToString(connectingSides, 2);
    }
}
