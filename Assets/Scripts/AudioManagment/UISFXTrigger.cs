using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISFXTrigger : MonoBehaviour
{
    private UIAudioSource source;

    void Start()
    {
        source = GameObject.Find("UIAudioSource").GetComponent<UIAudioSource>();
    }

    public void playHover()
    {
        source.sfxSource.playSound("button_hover");
    }

    public void playClick()
    {
        source.sfxSource.playSoundIfReady("button_press_0");
    }

    public void playClickMaster()
    {
        source.masterSource.playSoundIfReady("button_press_1");
    }

    public void playClickMusic()
    {
        source.musicSource.playSoundIfReady("piano_0");
    }
}
