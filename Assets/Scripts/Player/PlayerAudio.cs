using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource audioSource;

    public void playPickaxeSound()
    {
        audioSource.playSound("pickaxe" + Random.Range(0, 3).ToString(), 1.5f);
    }

    public void playFootstepSound()
    {
        audioSource.playSound("footstep" + Random.Range(0, 8).ToString(), 0.25f);
    }

    public void playFastFootstepSound()
    {
        audioSource.playSound("footstep" + Random.Range(0, 8).ToString(), 0.5f, 2f);
    }
}
