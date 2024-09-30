using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private BoxCollider boxCol;

    [SerializeField]
    private AudioSource audioSource;

    public void setOpen(bool state)
    {
        animator.SetBool("isOpen", state);
        boxCol.enabled = !state;
        audioSource.playSound("door_opening");
    }
}
