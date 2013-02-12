using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedSkyClassLibrary
{
    public abstract class AbstractFlightBehaviour: IFlightBehaviour
    {
        internal int transform;
        internal int fuel;
        internal int thrustValue;
        internal int maxSpeed;
        internal int currentSpeed;
        internal int target;
        internal int targetPosition;
        internal int targetVelocity;
        internal int fuelBurnRate;
        internal int atmosphericDrag;

        public void PitchUp()
        {
            throw new NotImplementedException();
        }

        public void PitchDown()
        {
            throw new NotImplementedException();
        }

        public void YawLeft()
        {
            throw new NotImplementedException();
        }

        public void YawRight()
        {
            throw new NotImplementedException();
        }

        public void RollLeft()
        {
            throw new NotImplementedException();
        }

        public void RollRight()
        {
            throw new NotImplementedException();
        }

        public void Accelerate()
        {
            throw new NotImplementedException();
        }

        public void Decelerate()
        {
            throw new NotImplementedException();
        }

        public void UpdateFuelRemaining();

        public void PredictTimeToLowFuel();
    }
}
