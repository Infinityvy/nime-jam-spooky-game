using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMaster
{
    public Dictionary<string, Team> users;

    public UserMaster()
    {
        users = new Dictionary<string, Team>();
    }

    public void joinUser(string name, Team team)
    {
        if (users.ContainsKey(name)) return;

        users.Add(name, team);
        Debug.Log("User joined: " + name + ", " + team.ToString());
    }
}
