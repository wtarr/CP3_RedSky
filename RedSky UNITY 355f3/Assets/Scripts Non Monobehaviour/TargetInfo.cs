using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TargetInfo : IComparable<TargetInfo>
{

    private String targetName;
    private Vector3 targetPosition;
    
    public String TargetName
    {
        get { return targetName; }
        set { targetName = value; }
    }

    public Vector3 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }
       
    public TargetInfo(string name, Vector3 pos)
    {
        this.targetName = name;
        this.targetPosition = pos;
    }

    public int CompareTo(TargetInfo other) 
    {      
        if (other.targetName.Equals(this.targetName))
            return 0;
        else
            return -1;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    

}
