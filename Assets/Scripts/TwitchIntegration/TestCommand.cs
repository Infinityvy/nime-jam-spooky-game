using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchIntegration;

public class TestCommand : TwitchMonoBehaviour
{
    [SerializeField] private GameObject _testObject;

    private Vector3 _targetPosition;

    [TwitchCommand("move_object", "move", "m")]
    public void moveObject(int x, int y)
    {
        _targetPosition = new Vector3(x, y, 3);
        _testObject.transform.position = _targetPosition;
    }

    [TwitchCommand("info")]
    public void info()
    {

    }
}
