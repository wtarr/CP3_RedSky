using System;
using UnityEngine;

public abstract class AbstractFlightBehaviour : IFlightBehaviour
{

    #region Class State
    GameObject gameObject;
    private float atmosphericDrag;
    private float currentSpeed;
    private float fuelRemaining;
    private float fuelBurnRate;
    private float maxSpeed;
    private Transform targetTransform;
    private Vector3 targetPosition;
    private Vector3 targetVelocityVector;
    private float targetSpeedMetersPerSecond;
    private float thrustValue; 
    #endregion

    #region Properties
    public float AtmosphericDrag
    {
        get { return atmosphericDrag; }
        set { atmosphericDrag = value; }
    }

    public float CurrentSpeed
    {
        get { return currentSpeed; }
        set { currentSpeed = value; }
    }

    public float FuelRemaining
    {
        get { return fuelRemaining; }
        set { fuelRemaining = value; }
    }

    public float FuelBurnRate
    {
        get { return fuelBurnRate; }
        set { fuelBurnRate = value; }
    }

    public float MaxSpeed
    {
        get { return maxSpeed; }
        set { maxSpeed = value; }
    }

    public Transform TargetTransform
    {
        get { return targetTransform; }
        set { targetTransform = value; }
    }

    public Vector3 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }

    public Vector3 TargetVelocityVector
    {
        get { return targetVelocityVector; }
        set { targetVelocityVector = value; }
    }

    public float TargetSpeedMetersPerSecond
    {
        get { return targetSpeedMetersPerSecond; }
        set { targetSpeedMetersPerSecond = value; }
    }
    
    public float ThrustValue
    {
        get { return thrustValue; }
        set { thrustValue = value; }
    } 
    #endregion

    
    public void Accelerate()
    {
        throw new NotImplementedException();
    }

    public void Decelerate()
    {
        throw new NotImplementedException();
    }

    public void PitchUp()
    {
        throw new NotImplementedException();
    }

    public void PitchDown()
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

    public void YawLeft()
    {
        throw new NotImplementedException();
    }

    public void YawRight()
    {
        throw new NotImplementedException();
    }        

    public void PredictTimeToLowFuel()
    {
        throw new NotImplementedException();
    }

    public void UpdateFuelRemaining()
    {
        throw new NotImplementedException();
    }
}
