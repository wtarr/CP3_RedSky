using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerCraft : AbstractFlightBehaviour, IAircraftBehaviour
{
    #region Class State
    private Missile[] missileStock;
    private int missileTotal = 4;    
    private int missileSelection = 0;
        
    #endregion

    #region Properties 
    public Missile[] MissileStock
    {
        get { return missileStock; }
        set { missileStock = value; }
    }

    public int MissileSelection
    {
        get { return missileSelection; }
        set { missileSelection = value; }
    }

    public int MissileTotal
    {
        get { return missileTotal; }
        set { missileTotal = value; }
    }
    #endregion

    public PlayerCraft()
    {
        missileStock = new Missile[missileTotal];

        for (int i = 0; i < missileTotal; i++)
        {            
            MissileStock[i] = new Missile();
        }

    }

    public void AquireMissileLock()
    {
        throw new NotImplementedException();
    }

    public void FireMissile()
    {        
        throw new NotImplementedException();
    }

    public void Radar()
    {
        throw new NotImplementedException();
    }

    public void DeployDecoyFlare()
    {
        throw new NotImplementedException();
    }

}
