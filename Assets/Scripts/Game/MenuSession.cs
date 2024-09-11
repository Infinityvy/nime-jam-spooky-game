using System.Collections;
using System.Collections.Generic;
using TwitchIntegration;
using UnityEngine;

public class MenuSession : MonoBehaviour
{
    public static MenuSession instance;
    public static bool isConnected = false;

    void Start()
    {
        instance = this;

        TwitchManager.OnTwitchClientFailedToConnect += onFailedToConnect;
        TwitchManager.OnTwitchClientJoinedChat += onClientConnect;
    }

    private void onFailedToConnect()
    {
        isConnected = false;
    }

    private void onClientConnect()
    {
        isConnected = true;
    }
}
