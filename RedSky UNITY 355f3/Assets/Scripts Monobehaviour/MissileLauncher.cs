using UnityEngine;
using System.Collections;

public class MissileLauncher : MonoBehaviour
{

    public Missile me;
    public GameObject missileRadar, missileRadarPrefab;
    Vector3 missileVelocityVectorToIntercept;
    Vector3 commonInterceptVector;
    float sweepAngleRate = 100;
    bool locked = false;
    Vector3 launched;
    Vector3 Cached;

    float timeOfLastCall;
    float timeNow;
    
  
    // Use this for initialization
    void Start()
    {


        missileRadar = (GameObject)Instantiate(missileRadarPrefab, transform.position, transform.rotation);
        missileRadar.transform.parent = transform;

        launched = transform.position;

        // calculate the intercept vector which is the target and missile will collide at time t based on missiles maxspeed
        commonInterceptVector = me.CalculateInterceptVector(me.PrimaryTarget.TargetPosition, me.TargetVelocityVector, me.Position, me.MaxSpeed);

        // calculate the velocity vector required for the missile to travel that will reach intercept
        missileVelocityVectorToIntercept = me.PlotCourse(commonInterceptVector, me.Position);
               
        // create a rigid body for our missile
        me.EntityObj.AddComponent<Rigidbody>();
        // create a sphere collider for our missile
        me.EntityObj.AddComponent<SphereCollider>();

        SphereCollider sc = (SphereCollider)me.EntityObj.collider;

        sc.radius = 0.5f; //set its intital det range

        me.EntityObj.rigidbody.useGravity = false;
        me.EntityObj.rigidbody.angularDrag = 0;
        me.EntityObj.rigidbody.mass = 1;


    }

    // Update is called once per frame
    void Update()
    {
        //Start sweeping
        missileRadar.transform.RotateAround(this.transform.position, this.transform.up, sweepAngleRate * Time.deltaTime);

        //If missile can lock on same target as player craft then launch!!!


        if (me.PrimaryTarget != null && locked == true)
        {
                       
            commonInterceptVector = me.CalculateInterceptVector(me.PrimaryTarget.TargetPosition, me.TargetVelocityVector, me.Position, me.MaxSpeed);

            Debug.DrawLine(launched, commonInterceptVector, Color.blue, 0.25f, false);

            missileVelocityVectorToIntercept = me.PlotCourse(commonInterceptVector, me.Position);
            
            // Check if path to intercept is still viable...
            // if not we will check if in detonation range
            // if not in detonation range we will continue on old velocity and hope for better intercept chance
            if (commonInterceptVector == Vector3.zero)
            {

                 //the path is not viable so lets check if missile is in detonation range of target					
                if (me.InDetonationRange(me.Position, me.PrimaryTarget.TargetPosition))
                {

                    SphereCollider myCollider = me.EntityObj.transform.GetComponent<SphereCollider>();
                    myCollider.radius = me.detonationRange;

                }

            }
            
        }
         
        me.EntityObj.transform.forward = Vector3.Normalize(missileVelocityVectorToIntercept);
        me.EntityObj.transform.position += missileVelocityVectorToIntercept * Time.deltaTime;
        
    }
        

    void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.name.Contains("reply") && other.gameObject.name.Equals(me.PrimaryTarget.TargetName))
        {                        
            locked = true;

            if (other.gameObject.transform.position != me.PrimaryTarget.TargetPosition)
            {
                timeNow = Time.realtimeSinceStartup;
                me.oldTargetPosition = me.PrimaryTarget.TargetPosition;
                me.PrimaryTarget.TargetPosition = other.gameObject.transform.position;
                if (timeNow > 0 && timeOfLastCall > 0)
                    me.TargetVelocityVector = me.CalculateVelocityVector(me.oldTargetPosition, me.PrimaryTarget.TargetPosition, (timeNow - timeOfLastCall));
                Debug.Log("Vel" + me.TargetVelocityVector);
                timeOfLastCall = timeNow;
            }
            

        }

    }


}
