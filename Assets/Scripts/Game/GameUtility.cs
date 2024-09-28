using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtility
{
    public static readonly float mineCycleDuration = 0.5f;

    public static void playSound(this AudioSource audioSource, string soundKey)
    {
        audioSource.clip = AudioManager.getAudioClip(soundKey);
        audioSource.Play();
    }
    public static void playSoundIfReady(this AudioSource audioSource, string soundKey) { if (!audioSource.isPlaying) audioSource.playSound(soundKey); }

    public static int loopIndex(int index, int max)
    {
        if (index < 0) return max - 1;
        else if(index >= max) return 0;
        else return index;
    }

    public static float getCursorAngleRelativeToPlayer()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        Vector3 mousePosition = clampMousePosition(Input.mousePosition);

        float xOffset = mousePosition.x - screenCenter.x;
        float zOffset = mousePosition.y - screenCenter.y;

        float angle = Vector3.Angle(Vector3.forward, new Vector3(xOffset, 0, zOffset));

        angle *= xOffset > 0 ? 1 : -1;

        angle += 45;

        return angle;
    }

    public static Vector3 clampMousePosition(Vector3 mousePosition)
    {
        float x = Mathf.Clamp(mousePosition.x, 0, Screen.width);
        float y = Mathf.Clamp(mousePosition.y, 0, Screen.height);

        return new Vector3(x, y, 0);
    }
}
