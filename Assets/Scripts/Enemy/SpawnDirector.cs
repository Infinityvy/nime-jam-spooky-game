using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDirector : MonoBehaviour
{
    private Transform hunterPrefab;

    private void Start()
    {
        hunterPrefab = Resources.Load<Transform>("Creatures/Hunter");

        WorldGenerator.instance.onFinishedGenerating += onWorldGenerated;
    }

    private void onWorldGenerated()
    {
        spawnEnemy(hunterPrefab, WorldGenerator.instance.getRandomNodePosition());
    }

    private void spawnEnemy(Transform prefab, Vector3 position)
    {
        Instantiate(prefab, position, Quaternion.identity);
    }
}
