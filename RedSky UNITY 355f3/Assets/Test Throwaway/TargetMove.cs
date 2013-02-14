using UnityEngine;
using System.Collections;

public class TargetMove : MonoBehaviour {
	
	public float speed;
	public float thrustVal;
	public Vector3 velocity, acceleration, oldPos, calculatedVel;
	
	// Use this for initialization
	void Start () {
		
			
		velocity = Vector3.zero;	
		
		thrustVal = 60f;
						
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
	
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 200, 20), "x: " + calculatedVel.x.ToString() + "y: " + calculatedVel.y.ToString() + "z: " + calculatedVel.z.ToString()); 
	}
}
