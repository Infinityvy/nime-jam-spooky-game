using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[System.Serializable]
public class Zone
{
    [SerializeField] private Vector3 minBounds;
    [SerializeField] private Vector3 maxBounds;

    public Zone(Vector3 minBounds, Vector3 maxBounds)
    {
        this.minBounds = minBounds;
        this.maxBounds = maxBounds;
    }

    public bool isPositionInZone(Vector3 pos)
    {
        return (pos.x >= minBounds.x && pos.x <= maxBounds.x) &&
               (pos.y >= minBounds.y && pos.y <= maxBounds.y) &&
               (pos.z >= minBounds.z && pos.z <= maxBounds.z);
    }

    public Vector3 getRandomPositionInZone()
    {
        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);
        float randomZ = Random.Range(minBounds.z, maxBounds.z);

        return new Vector3(randomX, randomY, randomZ);
    }

    public void rotate(Quaternion rotation)
    {
        minBounds = rotation * minBounds;
        maxBounds = rotation * maxBounds;
    }

    public (Vector3, Vector3) getBounds()
    {
        return (minBounds, maxBounds);
    }
}
