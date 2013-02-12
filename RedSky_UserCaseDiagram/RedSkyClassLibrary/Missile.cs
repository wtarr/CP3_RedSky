using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedSkyClassLibrary
{
    public class Missile: AbstractFlightBehaviour
    {


        public void ObtainRealTimeTargetsPosition();

        public void CalculateTargetsVelocity();
                
        public void PlotCourse();

        public void PredictIntercept();

        public void IsRouteFeasible();

        public bool InDetonationRange();

        public void CheckForPossibleCollisions();

    }
}
