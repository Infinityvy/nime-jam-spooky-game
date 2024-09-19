using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePrefab : MonoBehaviour
{
    [BitField]
    [Tooltip("Top, Right, Bottom, Left")]
    public int connectingSides;

    public Vector3[] nodes;
}
