using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedSkyClassLibrary
{
    public interface IAircraftBehaviour
    {
        void AquireMissileLock();

        void FireMissile();        

        void DeployDecoyFlare();
    }
}
