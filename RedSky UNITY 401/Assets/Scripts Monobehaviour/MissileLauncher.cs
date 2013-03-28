using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class MissileLauncher : MonoBehaviour
{
    public GameObject missileRadarPrefab, explosionPrefab;   
    private GameObject missileRadar;
    private Missile thisMissile;
    private GameObject owner;

    private Vector3 missileVelocityVectorToIntercept;
    private Vector3 commonInterceptVector;
    private float sweepAngleRate = 1000;
    private bool locked = false;
    private Vector3 launched;    
    private float timeOfLastCall;
    private float timeNow;
    
    public Missile ThisMissile
    {
        get { return thisMissile; }
        set { thisMissile = value; }
    }   

    public GameObject Owner
    {
        get { return owner; }
        set { owner = value; }
    }
  
    // Use this for initialization
    void Start()
    {

        if (Network.isServer)
        {
            missileRadar = (GameObject)Instantiate(missileRadarPrefab, transform.position, transform.rotation);
            missileRadar.transform.parent = transform;

            launched = transform.position;

            // calculate the intercept vector which is the target and missile will collide at time t based on missiles maxspeed

            commonInterceptVector = thisMissile.CalculateInterceptVector(thisMissile.PrimaryTarget.TargetPosition, thisMissile.TargetVelocityVector, thisMissile.Position, thisMissile.MaxSpeed);

            // calculate the velocity vector required for the missile to travel that will reach intercept
            missileVelocityVectorToIntercept = thisMissile.PlotCourse(commonInterceptVector, thisMissile.Position);

            // create a rigid body for our missile
            thisMissile.EntityObj.AddComponent<Rigidbody>();
            // create a sphere collider for our missile
            thisMissile.EntityObj.AddComponent<SphereCollider>();

            SphereCollider sc = (SphereCollider)thisMissile.EntityObj.collider;

            sc.radius = 0.5f; //set its intital det range
            sc.isTrigger = true;
            

            //sc.transform.parent = transform;

            thisMissile.EntityObj.rigidbody.useGravity = false;
            thisMissile.EntityObj.rigidbody.angularDrag = 0;
            thisMissile.EntityObj.rigidbody.mass = 1;

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Network.isServer)
        {
            //Start sweeping
            missileRadar.transform.RotateAround(this.transform.position, this.transform.up, sweepAngleRate * Time.deltaTime);

            //If missile can lock on same target as player craft then launch!!!


            if (thisMissile.PrimaryTarget != null)// && locked == true)
            {

                commonInterceptVector = thisMissile.CalculateInterceptVector(thisMissile.PrimaryTarget.TargetPosition, thisMissile.TargetVelocityVector, thisMissile.Position, thisMissile.MaxSpeed);

                Debug.DrawLine(launched, commonInterceptVector, Color.blue, 0.25f, false);

                missileVelocityVectorToIntercept = thisMissile.PlotCourse(commonInterceptVector, thisMissile.Position);

                // Check if path to intercept is still viable...
                // if not we will check if in detonation range
                // if not in detonation range we will continue on old velocity and hope for better intercept chance
                if (commonInterceptVector == Vector3.zero)
                {

                    //the path is not viable so lets check if missile is in detonation range of target					
                    if (thisMissile.InDetonationRange(thisMissile.Position, thisMissile.PrimaryTarget.TargetPosition))
                    {
                        Debug.Log("In det range");
                        SphereCollider myCollider = thisMissile.EntityObj.transform.GetComponent<SphereCollider>();
                        myCollider.radius = thisMissile.detonationRange;

                    }

                }

            }

            Debug.Log(missileVelocityVectorToIntercept);
            thisMissile.EntityObj.transform.forward = Vector3.Normalize(missileVelocityVectorToIntercept);
            thisMissile.EntityObj.transform.position += missileVelocityVectorToIntercept * Time.deltaTime;
            

        }
        
        
        
    }          

    void OnTriggerEnter(Collider other)
    {
        if (Network.isServer)
        {
            //if (other.transform.parent != null)
            //    Debug.Log("outer" + other.name + " " + other.gameObject.transform.parent.networkView.viewID.ToString().Split(' ').Last());

            if (other.gameObject.name.Contains("player_replying_to") &&
                other.gameObject.name.Contains("MissileRadar(Clone)") &&
                other.gameObject.transform.parent.networkView.viewID.ToString().Equals(thisMissile.PrimaryTarget.TargetID.ToString()))
            {
                Debug.Log("Recieving reply");
                locked = true;

                if (other.gameObject.transform.position != thisMissile.PrimaryTarget.TargetPosition)
                {
                    timeNow = Time.realtimeSinceStartup;
                    thisMissile.oldTargetPosition = thisMissile.PrimaryTarget.TargetPosition;
                    thisMissile.PrimaryTarget.TargetPosition = other.gameObject.transform.position;
                    if (timeNow > 0 && timeOfLastCall > 0)
                        thisMissile.TargetVelocityVector = thisMissile.CalculateVelocityVector(thisMissile.oldTargetPosition, thisMissile.PrimaryTarget.TargetPosition, (timeNow - timeOfLastCall));
                    //Debug.Log("Vel" + thisMissile.TargetVelocityVector);
                    timeOfLastCall = timeNow;
                }


            }
            else
            {
                locked = false;
            }
        }

    }

    





}

