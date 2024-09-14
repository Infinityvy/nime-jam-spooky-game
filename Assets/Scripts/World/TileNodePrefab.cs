using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNodePrefab : MonoBehaviour
{
    [BitField]
    [Tooltip("Top, Right, Bottom, Left")]
    public int connectingSides;
}
