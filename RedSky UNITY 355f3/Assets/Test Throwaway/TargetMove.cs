using UnityEngine;
using System;
using System.Collections;

public class TargetMove : MonoBehaviour {
	
	public float speed;
	public float thrustVal;
	public Vector3 velocity, acceleration, oldPos, calculatedVel;
	public GameObject exp, thisObject;
	
	// Use this for initialization
	void Start () {
		
			
		velocity = Vector3.zero;	
		
		thrustVal = 600f;
		
		thisObject = this.gameObject;
						
	}

	Vector3 CalculateVelocity (float delta)
	{
		return (transform.position - oldPos) / delta;
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
		acceleration = Vector3.zero;
		
		
		if (Input.GetKey(KeyCode.Space))
		{
			acceleration += thrustVal * transform.forward * Time.deltaTime;
			
		}
		
		
		velocity += acceleration * Time.deltaTime;
		
		
		transform.position += velocity * Time.deltaTime;
		
		calculatedVel = CalculateVelocity(Time.deltaTime);
		
		oldPos = transform.position;
	
	}
	
	void OnTriggerEnter(Collider other) {
       
		Debug.Log("Triggered");
		
		if (other.gameObject.name == "missile(Clone)" || other.gameObject.name == "missilefox2(Clone)" )
		{
			try {
				
				Instantiate(exp, transform.position, transform.rotation);
			
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
