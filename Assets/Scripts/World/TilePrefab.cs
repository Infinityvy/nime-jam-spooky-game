using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePrefab : MonoBehaviour
{
    public int connectingSides
    {
        get
        {
            int sides = 0;
            if (top) sides |= 0b1000;
            if (right) sides |= 0b0100;
            if (bottom) sides |= 0b0010;
            if (left) sides |= 0b0001;

            return sides;
        }
    }

    [SerializeField]
    private bool top, right, bottom, left;

    public Zone[] nodeZones = null;
}
