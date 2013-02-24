using UnityEngine;
using System;
using System.Collections;

public class TargetMove : MonoBehaviour {
	public EnemyCraft testCraft;	
	public float thrustVal;
	public Vector3 velocity, acceleration, oldPos, calculatedVel;
	public GameObject exp, thisObject;
	
	// Use this for initialization
	void Start () {
		
		testCraft = new EnemyCraft();
		
		testCraft.EntityObj = this.gameObject;
				
		testCraft.Velocity = Vector3.zero;
			
		testCraft.ThrustValue = 600f;
		
		testCraft.DecelerationValue = 300f;
		
		testCraft.PitchAngle = 0.01f;
		
		testCraft.YawAngle = 0.01f;
						
	}	
	
	// Update is called once per frame
	void Update () {
		
		
		
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
		
		
		//testCraft.Acceleration += testCraft.ThrustValue * (testCraft.EntityObj.transform.up * -1) * Time.deltaTime;
		
		testCraft.Velocity += testCraft.Acceleration * Time.deltaTime;
		
		testCraft.EntityObj.transform.position += testCraft.Velocity * Time.deltaTime;
		
		calculatedVel = testCraft.CalculateVelocityVector(oldPos, testCraft.Position, Time.deltaTime);
		
		oldPos = testCraft.Position;
	
	}
	
	void OnTriggerEnter(Collider other) {
       
		Debug.Log("Triggered");
		
		if (other.gameObject.name == "missile(Clone)" || other.gameObject.name == "missilefox2(Clone)" )
		{
			try {
				
				Instantiate(exp, testCraft.Position, testCraft.EntityObj.transform.rotation);
			
				Destroy(other.gameObject);
			
				Destroy(GameObject.Find("target"));
			}
			catch (Exception e)
			{
				Debug.Log(e.ToString());
			}
		}
    }
	
	
	
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 200, 20), "Targets Calculated Velocity"); 
		GUI.Label(new Rect(10, 30, 200, 20), "X: " + calculatedVel.x.ToString() + "    Y: " + calculatedVel.y.ToString() + "    Z: " + calculatedVel.z.ToString()); 
	}
}
