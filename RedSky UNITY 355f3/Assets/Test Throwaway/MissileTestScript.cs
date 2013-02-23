using UnityEngine;
using System.Collections;
using System;

public class MissileTestScript : MonoBehaviour
{
	
	Vector3 intercept;
	public GameObject explosion, missile, fox2, basestation;
	public Missile m;
	GameObject target;
	public float speed;
	Vector3 interceptforward, to;
	Vector3 oldTarPos, targVelocity, something;
	bool launch = false, launched = false;
	
	
	// Use this for initialization
	void Start ()
	{
		
		m = new Missile ();
		
		speed = 60f;
		
		m.MaxSpeed = speed;		
		
		
		target = GameObject.Find ("target");
		
		m.newMissilePosition = basestation.transform.position;
		
		
	}
	
	void OnGUI ()
	{
		GUI.Label (new Rect (10, 50, 80, 20), "Missile Max Speed");
		
		speed = float.Parse (GUI.TextField (new Rect (90, 50, 40, 20), speed.ToString ()));
		
		if (GUI.Button (new Rect (10, 70, 20, 20), "+"))
			speed += 10;
		if (GUI.Button (new Rect (40, 70, 20, 20), "-"))
			speed -= 10;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		m.MaxSpeed = speed;
		
		
		if (GameObject.Find ("target")) { //check that target still exists
			
			//basestation.transform.LookAt(target.transform);
			
			if (fox2 == null)
				m.newMissilePosition = basestation.transform.position;
			else
				m.newMissilePosition = fox2.transform.position;

			m.TargetPosition = target.transform.position;
			
			targVelocity = m.CalculateVelocityVector(oldTarPos, m.TargetPosition, Time.deltaTime);
			
			if (Input.GetKey (KeyCode.F)) {
			            		            			
						
				launch = true;
				
			
			}

			if (launch && !launched) {
				
				launch = false;
				
				// my missile
				fox2 = (GameObject)Instantiate (missile, basestation.transform.position, transform.rotation);
				
				// calculate the intercept vector which is the target and missile will collide at time t based on missiles maxspeed
				Vector3 interceptVector = m.CalculateInterceptVector(m.TargetPosition, targVelocity, m.newMissilePosition, m.MaxSpeed);
				
				// calculate the velocity vector required for the missile to travel that will reach intercept
				Vector3 missileVelocityVectorToIntercept = m.PlotCourse(interceptVector);
				
				to = missileVelocityVectorToIntercept;
				
				something = m.CalculateInterceptVector(m.TargetPosition, targVelocity, m.newMissilePosition, m.MaxSpeed);
				
				// create a rigid body for our missile
				fox2.AddComponent<Rigidbody> ();
				// create a sphere collider for our missile
				fox2.AddComponent<SphereCollider>();				                 
				
				SphereCollider sc = (SphereCollider)fox2.collider;
				
				sc.radius = 0.5f; //set its intital det range
				
				fox2.rigidbody.useGravity = false;
				fox2.rigidbody.angularDrag = 0;
				fox2.rigidbody.mass = 1;											
				
				launched = true;
			}
								
			if (launched && fox2 != null)
			{
				//recalculate
				
				Vector3 interceptVector = m.CalculateInterceptVector(m.TargetPosition, targVelocity, m.newMissilePosition, m.MaxSpeed);
				
				Vector3 missileVelocityVectorToIntercept = m.PlotCourse(interceptVector);
				
				
				// Check if path to intercept is still viable...
				// if not we will check if in detonation range
				// if not in detonation range we will continue on old velocity and hope for better intercept chance
				if (interceptVector == Vector3.zero)
				{
					
					// the path is not viable so lets check if missile is in detonation range of target					
					if (m.InDetonationRange())
					{
						
						
						SphereCollider myCollider = fox2.transform.GetComponent<SphereCollider>();
						myCollider.radius = m.detonationRange;
						
						
					}				
					
				}				
				else
				{	
					// This path is viable so update missile
					to = missileVelocityVectorToIntercept; 
				}	
				
				fox2.transform.forward = Vector3.Normalize(to);
				fox2.transform.position += to * Time.deltaTime;
				
				Debug.DrawLine(basestation.transform.position, something, Color.blue, 10, false);
				Debug.DrawLine(Vector3.zero, something, Color.white, 10, false);
			}

			
			oldTarPos = m.TargetPosition;		
		}
	}	
	
}
