using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedSkyClassLibrary
{
    public interface IFlightBehaviour
    {
        void PitchUp();

        void PitchDown();

        void YawLeft();

        void YawRight();

        void RollLeft();

        void RollRight();

        void Accelerate();

        void Decelerate();

        
        
    }
}
