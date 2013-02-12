using System;
using System.Collections.Generic;
using UnityEngine;

public class Missile: AbstractFlightBehaviour
{
    public List<UnityEngine.Vector3> FlightPath;
    public Vector3 oldTargetPosition, newTargetPosition, newMissilePosition, oldMissilePosition;
    public float timeSinceLastUpdate;
    public float linearDeviationTolerence = 0f; // for now
    public float detonationRange = 30f;

    public Missile()
    {
        FlightPath = new List<Vector3>();
        MaxSpeed = 20f;
    }
    
    public void ObtainRealTimeTargetsPosition()
    {
        // Radar ping and calculate current target position
    }

    public void CalculateTargetsVelocityandSpeed()
    {
        TargetVelocityVector = newTargetPosition - oldTargetPosition;

        FlightPath.Add(TargetVelocityVector);

        float vectorMagnitude = Vector3.Magnitude(TargetVelocityVector);

        TargetSpeedMetersPerSecond = vectorMagnitude / timeSinceLastUpdate;
    }

    public void PredictIntercept(int timeSlot)
    {
        Vector3 targetVelocityPrediction, targetPredictedPositionVector, missileHeadingToIntercept;

        //after 1 time targetPrediction = TargetVelocityVector
        //after 2 time targetPrediction = 2 * TargetVelocityVector
        // ...

        // |---->| known
        // |----------->| predicted

        targetVelocityPrediction = TargetVelocityVector * timeSlot;
        targetPredictedPositionVector = oldTargetPosition + targetVelocityPrediction;

        missileHeadingToIntercept = targetPredictedPositionVector - newMissilePosition;

        //Now test if this is feasible
        //if not increase time slot

        
    }

    public Vector3 CalculateInterceptVector()
    {
        // This calculation will be performed by the planes onboard system
        
        Vector3 o = newTargetPosition - newMissilePosition;


        double a = Math.Pow(TargetVelocityVector.x, 2) + Math.Pow(TargetVelocityVector.y, 2) + Math.Pow(TargetVelocityVector.z, 2) - Math.Pow(MaxSpeed, 2);
		
		if (a == 0) a = 0.001; // avoid a div by zero

        double b = (o.x * TargetVelocityVector.x) + (o.y * TargetVelocityVector.y) + (o.z * TargetVelocityVector.z);

        double c = Math.Pow(o.x, 2) + Math.Pow(o.y, 2) + +Math.Pow(o.z, 2);

        double desc = Math.Pow(b, 2) - (a * c);
		
		if (desc < 0)
			Debug.Log ("negative");
		
		double t1 = (-b + Math.Sqrt( Math.Pow(b, 2) - (a * c) )) / a;
		double t2 = (-b - Math.Sqrt( Math.Pow(b, 2) - (a * c) )) / a;
        
        //double t1 = (b + (Math.Sqrt(Math.Pow(b, 2) - (4 * a * c)))) / (2 * a);
        //double t2 = (b - (Math.Sqrt(Math.Pow(b, 2) - (4 * a * c)))) / (2 * a);              
        
		float t = 1;
		
		if (t1 < 0)
			t = (float)t2;
		
		if (t2 < 0) // all hope is lost
			return new Vector3(0,0,0);
		
		if (t1 >= 0 && t2 >= 0)
			t = (float)Math.Min(t1, t2);		
        
        Vector3 intercept = TargetPosition + (TargetVelocityVector * t);

       	return Vector3.Normalize(intercept - newMissilePosition);
    }

    public bool IsTargetCourseLinear()
    {
        Vector3 last, secondLast, thirdLast;

        if (FlightPath.Count >= 3)
        {
            last = FlightPath[FlightPath.Count - 1];
            secondLast = FlightPath[FlightPath.Count - 2];
            thirdLast = FlightPath[FlightPath.Count - 3];

            float angleBetweenThirdandSecondLast = Vector3.Angle(thirdLast, secondLast);
            float angleBetweenSecondLastandLast = Vector3.Angle(secondLast, last);

            if (angleBetweenSecondLastandLast == linearDeviationTolerence && angleBetweenThirdandSecondLast == linearDeviationTolerence)
                return true;
            else
                return false;
        }

        return false;

        
    }

    public void PlotCourse()
    {
        
    }

    public void IsRouteFeasible()
    {
        //based on fuel load and speed is it possible to make this intercept
    }

    public bool InDetonationRange()
    {
        
        float distance = Vector3.Distance(TargetPosition, newMissilePosition);

        if (distance <= detonationRange)
            return true;
        else       
            return false;
    }

    public void CheckForPossibleCollisions()
    {
        
    }
}
