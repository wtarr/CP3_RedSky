using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class PlayerInfo
{
    public PlayerInfo()
    {

    }

    public PlayerInfo(string playerName, NetworkViewID viewID)
    {
        this.playerName = playerName;
        this.viewID = viewID;
    }

    string playerName;
    NetworkViewID viewID;

    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }

    public NetworkViewID ViewID
    {
        get { return viewID; }
        set { viewID = value; }
    }
}

