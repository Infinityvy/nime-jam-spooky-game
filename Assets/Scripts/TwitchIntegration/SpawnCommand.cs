using System.Collections;
using System.Collections.Generic;
using TwitchIntegration;
using UnityEngine;

public class SpawnCommand : TwitchMonoBehaviour
{
    public Transform spawnObject;

    [TwitchCommand("spawn")]
    public void spawn()
    {
        Transform tmp = Instantiate(spawnObject, Vector3.zero, Quaternion.identity);
    }
}
