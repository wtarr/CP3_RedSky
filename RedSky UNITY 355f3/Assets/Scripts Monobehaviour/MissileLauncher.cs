using UnityEngine;
using System.Collections;

public class MissileLauncher : MonoBehaviour
{

    public Missile me;
    public GameObject missileRadar, missileRadarPrefab;
    Vector3 to, interceptVector;
    float sweepAngleRate = 100;
    bool locked = false;
    Vector3 launched;
    Vector3 Cached;

    float lastcall;
    float now;
    float diff;
  
    // Use this for initialization
    void Start()
    {


        missileRadar = (GameObject)Instantiate(missileRadarPrefab, transform.position, transform.rotation);
        missileRadar.transform.parent = transform;

        launched = transform.position;

        // calculate the intercept vector which is the target and missile will collide at time t based on missiles maxspeed
        interceptVector = me.CalculateInterceptVector(me.TargetPosition, me.TargetVelocityVector, me.Position, me.MaxSpeed);

        // calculate the velocity vector required for the missile to travel that will reach intercept
        Vector3 missileVelocityVectorToIntercept = me.PlotCourse(interceptVector);

        to = missileVelocityVectorToIntercept;

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

            //inflight
            //recalculate

            //Debug.Log("old" + me.oldTargetPosition);
            //Debug.Log("curr" + me.PrimaryTarget.TargetPosition);
           
            //me.TargetVelocityVector = me.CalculateVelocityVector(me.oldTargetPosition, me.PrimaryTarget.TargetPosition, Time.deltaTime);

            

            interceptVector = me.CalculateInterceptVector(me.PrimaryTarget.TargetPosition, me.TargetVelocityVector, me.Position, me.MaxSpeed);

            Debug.DrawLine(launched, interceptVector, Color.blue, 1, false);

            Vector3 missileVelocityVectorToIntercept = me.PlotCourse(interceptVector);
            
            // Check if path to intercept is still viable...
            // if not we will check if in detonation range
            // if not in detonation range we will continue on old velocity and hope for better intercept chance
            if (interceptVector == Vector3.zero)
            {

                 //the path is not viable so lets check if missile is in detonation range of target					
                if (me.InDetonationRange())
                {

                    SphereCollider myCollider = me.EntityObj.transform.GetComponent<SphereCollider>();
                    myCollider.radius = me.detonationRange;

                }

            }
            else
            {
                 //This path is viable so update missile
                to = missileVelocityVectorToIntercept;
                Debug.Log(Vector3.Magnitude(missileVelocityVectorToIntercept));
            }

        }
         
        me.EntityObj.transform.forward = Vector3.Normalize(to);
        me.EntityObj.transform.position += to * Time.deltaTime;


        

    }
        

    void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.name.Contains("reply") && other.gameObject.name.Equals(me.PrimaryTarget.TargetName))
        {                        
            locked = true;

            if (other.gameObject.transform.position != me.PrimaryTarget.TargetPosition)
            {
                now = Time.realtimeSinceStartup;
                me.oldTargetPosition = me.PrimaryTarget.TargetPosition;
                me.PrimaryTarget.TargetPosition = other.gameObject.transform.position;
                if (now > 0 && lastcall > 0)
                    me.TargetVelocityVector = me.CalculateVelocityVector(me.oldTargetPosition, me.PrimaryTarget.TargetPosition, (now - lastcall));
                Debug.Log("Vel" + me.TargetVelocityVector);
                lastcall = now;
            }
            

        }

    }


}
