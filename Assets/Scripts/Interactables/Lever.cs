using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IInteractable = Interfaces.IInteractable;

public class Lever : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Door door;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AudioSource audioSource;

    private bool isUp = false;

    public Vector3 getHighlightButtonPos()
    {
        return transform.position + Vector3.up;
    }

    public bool Interact(PlayerController playerController)
    {
        if(isUp)
        {
            door.setOpen(false);
            animator.SetBool("isUp", false);
            isUp = false;
            audioSource.playSound("lever_pull_down", 1.5f);
        }
        else
        {
            door.setOpen(true);
            animator.SetBool("isUp", true);
            isUp = true;
            audioSource.playSound("lever_pull_up", 1.5f);
        }

        return false;
    }
}
