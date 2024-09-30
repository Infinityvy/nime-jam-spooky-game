using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public ParticleSystem sparkParticles;

    public void playPickaxeSound()
    {
        audioSource.playSound("pickaxe" + Random.Range(0, 3).ToString(), 1.5f);
        sparkParticles.Play();
    }
    public void playPickaxeWooshSound()
    {
        audioSource.playSound("woosh" + Random.Range(0, 2).ToString());
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
