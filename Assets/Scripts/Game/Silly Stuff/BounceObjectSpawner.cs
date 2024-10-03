using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObjectSpawner : MonoBehaviour
{
    public Transform bounceObject;

    private Vector3 spawnPos;
    private Quaternion spawnRot;

    private float spawnInterval = 3f;
    private int maxObjectCount = 15;
    private int objectCount = 1;

    private void Start()
    {
        spawnPos = bounceObject.position;
        spawnRot = bounceObject.rotation;

        InvokeRepeating(nameof(spawnBounceObject), spawnInterval, spawnInterval);
    }

    private void spawnBounceObject()
    {
        if (objectCount >= maxObjectCount) return;
        objectCount++;

        Instantiate(bounceObject, spawnPos, spawnRot, transform);
    }
}
