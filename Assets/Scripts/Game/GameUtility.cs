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
}
