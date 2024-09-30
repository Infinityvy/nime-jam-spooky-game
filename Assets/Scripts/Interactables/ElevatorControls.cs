using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using IInteractable = Interfaces.IInteractable;

public class ElevatorControls : MonoBehaviour, IInteractable
{
    private bool isReadyToUse = false;

    private void Start()
    {
        WorldGenerator.instance.onFinishedGenerating += setReadyToUse;
    }

    public Vector3 getHighlightButtonPos()
    {
        return transform.position + Vector3.up * 0.6f;
    }

    public bool Interact(PlayerController playerController)
    {
        if (!isReadyToUse) return false;

        Session.instance.finalizeLevel(false);

        return false;
    }

    private void setReadyToUse()
    {
        isReadyToUse = true;
    }
}
