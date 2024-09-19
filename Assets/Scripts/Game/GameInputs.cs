using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInputs
{
    public static Dictionary<string, KeyCode> keys;

    public static void initiate()
    {
        keys = new Dictionary<string, KeyCode>();

        keys.TryAdd("Forward", KeyCode.W);
        keys.TryAdd("Left", KeyCode.A);
        keys.TryAdd("Right", KeyCode.D);
        keys.TryAdd("Back", KeyCode.S);
        keys.TryAdd("Sprint", KeyCode.LeftShift);
    }
}
