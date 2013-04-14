using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class PlayerInfo
{
    private string playerName;
    private NetworkViewID viewID;

    public PlayerInfo()
    {

    }

    public PlayerInfo(string playerName, NetworkViewID viewID)
    {
        this.playerName = playerName;
        this.viewID = viewID;
    }        

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

