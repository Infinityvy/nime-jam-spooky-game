using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtility
{
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
}
