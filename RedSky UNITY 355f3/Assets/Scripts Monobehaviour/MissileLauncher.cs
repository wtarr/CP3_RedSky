using UnityEngine;
using System.Collections;

public class MissileLauncher : MonoBehaviour
{

    public Missile me;
    Vector3 to, interceptVector;

    // Use this for initialization
    void Start()
    {

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

        if (me.PrimaryTarget != null)
        {

            //inflight
            //recalculate
            me.TargetPosition = me.PrimaryTarget.transform.position;

            me.TargetVelocityVector = me.CalculateVelocityVector(me.oldTargetPosition, me.TargetPosition, Time.deltaTime);

            interceptVector = me.CalculateInterceptVector(me.TargetPosition, me.TargetVelocityVector, me.Position, me.MaxSpeed);

            Vector3 missileVelocityVectorToIntercept = me.PlotCourse(interceptVector);


            // Check if path to intercept is still viable...
            // if not we will check if in detonation range
            // if not in detonation range we will continue on old velocity and hope for better intercept chance
            if (interceptVector == Vector3.zero)
            {

                // the path is not viable so lets check if missile is in detonation range of target					
                if (me.InDetonationRange())
                {

                    SphereCollider myCollider = me.EntityObj.transform.GetComponent<SphereCollider>();
                    myCollider.radius = me.detonationRange;

                }

            }
            else
            {
                // This path is viable so update missile
                to = missileVelocityVectorToIntercept;
            }

        }

        me.EntityObj.transform.forward = Vector3.Normalize(to);
        me.EntityObj.transform.position += to * Time.deltaTime;


        me.oldTargetPosition = me.TargetPosition;

    }



}
