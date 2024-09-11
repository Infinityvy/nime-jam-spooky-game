using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;
using TwitchIntegration;
using UnityEngine.UI;
using static TwitchIntegration.Utils.TwitchVariables;
using TwitchIntegration.Utils;

public class UIAuthLogic : MonoBehaviour
{
    [SerializeField] private TMP_InputField channelNameField;
    [SerializeField] private TextMeshProUGUI channelNamePlaceholder;
    [SerializeField] private TextMeshProUGUI resultText;

    private string authSuccess = "Successfully authenticated Twitch account!";
    private string authRequired = "Authentication timed out!";

    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button playButton;

    [SerializeField] private Color green;
    [SerializeField] private Color red;

    private string connected = "Connected";
    private string notConnected = "Attempting to connect";

    private void Start()
    {
        if(PlayerPrefs.HasKey("TwitchAuth__ChannelName"))
        {
            channelNamePlaceholder.text = PlayerPrefs.GetString("TwitchAuth__ChannelName");
        }

        InvokeRepeating(nameof(updateStatusText), 0.2f, 0.2f);
    }

    public void onAuthenticateButton()
    {
        channelNamePlaceholder.text = channelNameField.text;

        TwitchManager.Authenticate(channelNameField.text, channelNameField.text, isAuthenticated =>
        {
            if(isAuthenticated)
            {
                resultText.text = authSuccess;
                resultText.color = green;
            }
            else
            {
                resultText.text = authRequired;
                resultText.color = red;
            }
        });
    }

    private void updateStatusText()
    {

        if(MenuSession.isConnected)
        {
            statusText.text = connected;
            statusText.color = green;
            playButton.interactable = true;
        }
        else
        {
            statusText.text = notConnected + generateDots();
            statusText.color = red;
            playButton.interactable = false;
        }
    }

    private int spaceIndex = 0;
    private string generateDots()
    {
        string text = "";

        for (int i = 0; i < 3; i++)
        {
            if (i == spaceIndex) text += " ";
            else text += ".";
        }

        spaceIndex = (spaceIndex + 1) % 3;

        return text;
    }
}
