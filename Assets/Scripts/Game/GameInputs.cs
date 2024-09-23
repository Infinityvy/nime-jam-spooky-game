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


        keys.TryAdd("Interact", KeyCode.E);
        keys.TryAdd("Drop Item", KeyCode.Q);
        keys.TryAdd("Use Item", KeyCode.Mouse0);
        keys.TryAdd("Slot 1", KeyCode.Alpha1);
        keys.TryAdd("Slot 2", KeyCode.Alpha2);
        keys.TryAdd("Slot 3", KeyCode.Alpha3);
        keys.TryAdd("Slot 4", KeyCode.Alpha4);
    }
}
