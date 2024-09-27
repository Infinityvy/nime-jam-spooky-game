using System;
using System.Collections;
using System.Collections.Generic;
using TwitchIntegration;
using UnityEngine;

public class Session : MonoBehaviour
{
    public static Session instance { get; private set; }

    public bool paused { private set; get; } = false;

    public UserMaster userMaster;
    public TwitchUser lastUser;

    void Start()
    {
        instance = this;

        userMaster = new UserMaster();

        GameInputs.initiate();

        Cursor.lockState = CursorLockMode.Confined;

        //TwitchManager.OnTwitchCommandReceived += onCommand;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            togglePauseMenu(!paused);
        }
    }

    private void onCommand(TwitchUser user, TwitchCommand command)
    {
        lastUser = user;
    }

    private Team teamToJoin = Team.ONE;
    public Team getTeamToJoin()
    {
        Team team = teamToJoin;
        teamToJoin++;
        if (teamToJoin > Team.FOUR) teamToJoin = Team.ONE;

        return team;
    }

    private void togglePauseMenu(bool state)
    {
        UIMenuLogic.instance.toggleMenu(state);
        togglePaused(state);
    }

    public void togglePaused(bool state)
    {
        if(state)
        {
            Time.timeScale = 0f;
            paused = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            paused = false;
        }
    }
}
