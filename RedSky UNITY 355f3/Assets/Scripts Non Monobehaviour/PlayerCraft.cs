using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public struct TargetInfo
{
    public TargetInfo(string name, Vector3 pos)
    {
        this.targetName = name;
        this.targetPosition = pos;
    }

    public String targetName;
    public Vector3 targetPosition;
}

public class PlayerCraft : AbstractFlightBehaviour, IAircraftBehaviour
{
        public Missile[] missileStock;        

        public PlayerCraft()
        {
            missileStock = new Missile[4];



            Missile m1 = new Missile();
            Missile m2 = new Missile();
            Missile m3 = new Missile();
            Missile m4 = new Missile();

            missileStock[0] = m1;
            missileStock[1] = m2;
            missileStock[2] = m3;
            missileStock[3] = m4;           

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
