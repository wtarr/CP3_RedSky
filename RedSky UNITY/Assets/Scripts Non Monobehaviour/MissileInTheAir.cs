using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MissileInTheAir
{
    private NetworkViewID theMissileId;
    private NetworkViewID theTargetId;
    private NetworkViewID theLaunchersId;

    public MissileInTheAir(NetworkViewID missileId, NetworkViewID targetId, NetworkViewID launchersId)
    {
        theMissileId = missileId;
        theTargetId = targetId;
        theLaunchersId = launchersId;
    }

    public NetworkViewID TheMissileId
    {
        get { return theMissileId; }
        set { theMissileId = value; }
    }

    public NetworkViewID TheTargetId
    {
        get { return theTargetId; }
        set { theTargetId = value; }
    }

    public NetworkViewID TheLaunchersId
    {
        get { return theLaunchersId; }
        set { theLaunchersId = value; }
    }
}

