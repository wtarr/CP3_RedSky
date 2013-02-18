using System;
using System.Collections.Generic;
using UnityEngine;

public class Missile: AbstractFlightBehaviour
{
    public List<UnityEngine.Vector3> FlightPath;
    public Vector3 oldTargetPosition, newMissilePosition, oldMissilePosition;    
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
	
	
    public Vector3 CalculateInterceptVector(Vector3 targPos, Vector3 targVelocity, float missileMaxSpeed)
    {
        // This calculation will be performed by the planes onboard system
        
        Vector3 o = targPos - Vector3.zero; // for simplification purposes

        double a = Math.Pow(targVelocity.x, 2) + Math.Pow(targVelocity.y, 2) + Math.Pow(targVelocity.z, 2) - Math.Pow(missileMaxSpeed, 2);
		
		if (a == 0) a = 0.000001f; // avoid a div by zero

        double b = (o.x * targVelocity.x) + (o.y * targVelocity.y) + (o.z * targVelocity.z);

        double c = Math.Pow(o.x, 2) + Math.Pow(o.y, 2) + +Math.Pow(o.z, 2);

        double desc = Math.Pow(b, 2) - (a * c);
		
		if (desc < 0)
			Debug.Log ("negative");
		
		double t1 = (-b + Math.Sqrt( Math.Pow(b, 2) - (a * c) )) / a;
		double t2 = (-b - Math.Sqrt( Math.Pow(b, 2) - (a * c) )) / a;
        
             
        
		float t = 1;
		
		if (t1 < 0)
			t = (float)t2;
		
		if (t2 < 0) // all hope is lost
			return new Vector3(0,0,0);
		
		if (t1 >= 0 && t2 >= 0)
			t = (float)Math.Min(t1, t2);		
		
		//Debug.Log("T " + t);
        
        //Vector3 intercept = TargetPosition + (TargetVelocityVector * t);
		Vector3 intercept = targVelocity + (targPos / t);
		
       	return intercept;
    }
	
	public Vector3 CalculateInterceptVector(Vector3 targPos, Vector3 targVelocity, Vector3 firingbasePos, float missileMaxSpeed)
    {
        // This calculation will be performed by the planes onboard system
        
        Vector3 o = targPos - firingbasePos; // for simplification purposes

        double a = Math.Pow(targVelocity.x, 2) + Math.Pow(targVelocity.y, 2) + Math.Pow(targVelocity.z, 2) - Math.Pow(missileMaxSpeed, 2);
		
		if (a == 0) a = 0.000001f; // avoid a div by zero

        double b = (o.x * targVelocity.x) + (o.y * targVelocity.y) + (o.z * targVelocity.z);

        double c = Math.Pow(o.x, 2) + Math.Pow(o.y, 2) + +Math.Pow(o.z, 2);

        double desc = Math.Pow(b, 2) - (a * c);
		
		if (desc < 0)
			Debug.Log ("negative");
		
		double t1 = (-b + Math.Sqrt( Math.Pow(b, 2) - (a * c) )) / a;
		double t2 = (-b - Math.Sqrt( Math.Pow(b, 2) - (a * c) )) / a;
        
             
        
		float t = 1;
		
		if (t1 < 0)
			t = (float)t2;
		
		if (t2 < 0) // all hope is lost
			return new Vector3(0,0,0);
		
		if (t1 >= 0 && t2 >= 0)
			t = (float)Math.Min(t1, t2);		
		
		//Debug.Log("T " + t);
        
        Vector3 intercept = targPos + (targVelocity * t);
		//Vector3 intercept = targVelocity + (targPos / t);
		
       	return intercept;
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
