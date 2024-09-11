using System;
using System.Collections;
using System.Collections.Generic;
using TwitchIntegration;
using UnityEngine;

public class Session : MonoBehaviour
{
    public static Session instance { get; private set; }

    public UserMaster userMaster;
    public TwitchUser lastUser;

    void Start()
    {
        instance = this;

        userMaster = new UserMaster();

        //TwitchManager.OnTwitchCommandReceived += onCommand;
    }

    void Update()
    {
        
    }

    void onCommand(TwitchUser user, TwitchCommand command)
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
}
