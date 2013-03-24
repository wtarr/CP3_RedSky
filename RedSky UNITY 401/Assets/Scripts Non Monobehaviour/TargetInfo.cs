using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TargetInfo : IComparable<TargetInfo>
{

    private NetworkViewID targetID;
    private Vector3 targetPosition;
    private bool isPrimary;

    public bool IsPrimary
    {
        get { return isPrimary; }
        set { isPrimary = value; }
    }
    
    public NetworkViewID TargetID
    {
        get { return targetID; }
        set { targetID = value; }
    }

    public Vector3 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }
       
    public TargetInfo(NetworkViewID name, Vector3 pos)
    {
        this.targetID = name;
        this.targetPosition = pos;
    }

    public int CompareTo(TargetInfo other) 
    {      
        if (other.TargetID.ToString().Equals(this.TargetID.ToString()))
            return 0;
        else
            return -1;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    

}
