using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyLauncher : MonoBehaviour {

    public EnemyCraft testCraft;
    public float thrustVal;
    public Vector3 velocity, acceleration, oldPos, calculatedVel;
    public GameObject explosionPrefab, pingReplyPrefab;   

    // Use this for initialization
    void Start()
    {

        testCraft = new EnemyCraft();

        testCraft.EntityObj = this.gameObject;

        testCraft.Velocity = Vector3.zero;

        testCraft.ThrustValue = 600f;

        testCraft.DecelerationValue = 300f;

        testCraft.PitchAngle = 0.01f;

        testCraft.YawAngle = 0.01f;
                
    }

    // Update is called once per frame
    void Update()
    {

        #region Movement
        testCraft.Acceleration = Vector3.zero;


        if (Input.GetKey(KeyCode.D)) // forward
        {
            testCraft.Accelerate();

        }

        if (Input.GetKey(KeyCode.W)) // pitch up
        {
            testCraft.PitchUp();

        }

        if (Input.GetKey(KeyCode.S)) // pitch down
        {
            testCraft.PitchDown();

        }

        if (Input.GetKey(KeyCode.A)) // break/reverse
        {
            testCraft.Decelerate();

        }

        if (Input.GetKey(KeyCode.Q)) // yaw left
        {
            testCraft.YawLeft();

        }

        if (Input.GetKey(KeyCode.E)) // yaw right
        {
            testCraft.YawRight();

        } 
        #endregion


        //testCraft.Acceleration += testCraft.ThrustValue * (testCraft.EntityObj.transform.up * -1) * Time.deltaTime;

        testCraft.Velocity += testCraft.Acceleration * Time.deltaTime;

        testCraft.EntityObj.transform.position += testCraft.Velocity * Time.deltaTime;

        calculatedVel = testCraft.CalculateVelocityVector(oldPos, testCraft.Position, Time.deltaTime);

        Debug.Log("CalcVel" + calculatedVel);


        oldPos = testCraft.Position;
        
       

    }

    void OnTriggerEnter(Collider other)
    {

        //Debug.Log(other.name);

        if ((other.gameObject.name.Contains("RadarSweep(Clone)") || other.gameObject.name.Contains("MissileRadar(Clone)")) && !other.gameObject.name.Contains("reply"))
        {
           
            GameObject temp = (GameObject)Instantiate(pingReplyPrefab, testCraft.Position, testCraft.Rotation);
            //Debug.Log(other.name.ToString());
            temp.GetComponent<Reply>().message = gameObject.name;//"reply_to_" + other.name.ToString() + Time.timeSinceLevelLoad.ToString();
            

            
        }

        if (other.gameObject.name == "AIM9Missile(Clone)")
        {
            try
            {

                Instantiate(explosionPrefab, testCraft.Position, testCraft.EntityObj.transform.rotation);

                Destroy(other.gameObject);

                Destroy(gameObject);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
    }


}