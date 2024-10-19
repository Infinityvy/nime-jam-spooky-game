using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDirector : MonoBehaviour
{
    private Transform[] monsterPrefabs;

    private void Start()
    {
        monsterPrefabs = Resources.LoadAll<Transform>("Creatures");

        WorldGenerator.instance.onFinishedGenerating += spawnRandomEnemy;

        Invoke(nameof(spawnRandomEnemy), 30);
        InvokeRepeating(nameof(spawnRandomEnemy), 90, 90);
    }

    private void spawnRandomEnemy()
    {
        spawnEnemy(monsterPrefabs[Random.Range(0, monsterPrefabs.Length)]);
    }

    private void spawnEnemy(Transform prefab)
    {
        Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }
}
