using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedSkyClassLibrary
{
    public class PlayerCraft: AbstractFlightBehaviour, IAircraftBehaviour
    {

        public Vector3 cameraPosition;


        public void UpdateCamera()
        {
            
        }

        public void ShouldPitchUp()
        {
            throw new NotImplementedException();
        }

        public void ShouldPitchDown()
        {
            throw new NotImplementedException();
        }

        public void ShouldYawLeft()
        {
            throw new NotImplementedException();
        }

        public void ShouldYawRight()
        {
            throw new NotImplementedException();
        }

        public void ShouldRollLeft()
        {
            throw new NotImplementedException();
        }

        public void ShouldRollRight()
        {
            throw new NotImplementedException();
        }

        public void ShouldAccelerate()
        {
            throw new NotImplementedException();
        }

        public void ShouldDecelerate()
        {
            throw new NotImplementedException();
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
}
