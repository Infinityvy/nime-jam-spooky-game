using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode
{
    public TileNode(Vector3Int tilePos) 
    {
        connectingSides = 0b0000;
        this.tilePos = tilePos;

        tileObject = GameObject.Instantiate(Resources.Load<Transform>("EndTiles/tile_empty"), tilePos * WorldGenerator.tileScale, Quaternion.identity, null);
        GameObject.Destroy(tileObject.GetComponent<TileNodePrefab>());

        tileObject.name += " " + Convert.ToString(connectingSides, 2) + " " + tilePos.ToString();
    }

    public TileNode(Transform prefab, Vector3Int tilePos, Quaternion rotation, Transform parent, int connectingSides)
    {
        this.tilePos = tilePos;
        this.connectingSides = connectingSides;

        tileObject = GameObject.Instantiate(prefab, tilePos * WorldGenerator.tileScale, rotation, parent);
        GameObject.Destroy(tileObject.GetComponent<TileNodePrefab>());

        tileObject.name += " " + Convert.ToString(connectingSides, 2) + " " + tilePos.ToString();
    }

    [BitField]
    [Tooltip("Top, Right, Bottom, Left")]
    public int connectingSides = 0b0000;

    public Transform tileObject = null;

    public Vector3Int tilePos = Vector3Int.zero;

    public override string ToString()
    {
        return System.Convert.ToString(connectingSides, 2);
    }
}
