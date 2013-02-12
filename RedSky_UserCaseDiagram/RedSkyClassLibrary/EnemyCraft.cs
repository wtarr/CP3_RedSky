using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedSkyClassLibrary
{
    public class EnemyCraft: AbstractFlightBehaviour, IAircraftBehaviour
    {
        
        public void AquireMissileLock();
        
        public void FireMissile();

        public void Radar();

        public void DeployDecoyFlare();

        public void PatrolArea();

        public void EvasiveManuevers();
        
    }
}
