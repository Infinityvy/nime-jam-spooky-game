using Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using IInteractable = Interfaces.IInteractable;

public class ElevatorControls : MonoBehaviour, IInteractable
{
    private ProgressBar progressBar;

    private bool isReadyToUse = false;
    private float progress = 0;

    private float timeWhenLastInteracted = 0;

    private void Start()
    {
        WorldGenerator.instance.onFinishedGenerating += setReadyToUse;
        progressBar = ProgressBar.instance;

        PlayerMovement.instance.playerIsMoving += disableProgress;
    }

    private void Update()
    {
        if(progress == 0) return;

        if(Time.time - timeWhenLastInteracted > GameUtility.mineCycleDuration * 2)
        {
            progress = 0;
            progressBar.setValue(0);
        }
    }

    public Vector3 getHighlightButtonPos()
    {
        return transform.position + Vector3.up * 0.6f;
    }

    public bool Interact(PlayerController playerController)
    {
        if (!isReadyToUse) return false;

        timeWhenLastInteracted = Time.time;

        progress += 0.26f;
        progressBar.gameObject.SetActive(true);
        progressBar.setPosition(getHighlightButtonPos() + Vector3.up * 0.4f);
        progressBar.setValue(progress);
        progressBar.setText("Starting Elevator...");

        PlayerMovement.instance.freezePlayerForDuration(0.1f);


        if(progress >= 1) Session.instance.finalizeLevel(false);

        return false;
    }

    private void setReadyToUse()
    {
        isReadyToUse = true;
    }

    private void disableProgress()
    {
        if(progressBar.gameObject.activeSelf) progressBar.gameObject.SetActive(false);
        progress = 0;
        progressBar.setValue(0);
    }
}
