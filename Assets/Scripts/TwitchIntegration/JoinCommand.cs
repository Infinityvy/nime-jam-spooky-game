using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchIntegration;

public class JoinCommand : TwitchMonoBehaviour
{
    [TwitchCommand("join")]
    public void join(TwitchUser user)
    {
        Team team = Session.instance.getTeamToJoin();

        Session.instance.userMaster.joinUser(user.displayname, team);
    }
}
