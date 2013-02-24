using UnityEngine;
using System.Collections;
using System;

public class LauncherCraft : MonoBehaviour
{
	public PlayerCraft playerCraft;		
	public Missile m;
		
	public GameObject explosion, missile; // prefabs
	
	
	Vector3 interceptforward, to;
	Vector3 oldTarPos, targVelocity, something;
	bool launch = false, launched = false;
	
	
	// Use this for initialization
	void Start ()
	{
		
		playerCraft = new PlayerCraft();
		
		playerCraft.EntityObj = this.gameObject;
		
		//set other player attributes here ,,, not releavant for now
		
		m = new Missile ();
		
//		m.MaxSpeed = speed;		
				
		playerCraft.PrimaryTarget = GameObject.Find ("target"); // Move
		
		//m.Position = basestation.transform.position;
		
		
	}
	
	void OnGUI ()
	{
		GUI.Label (new Rect (10, 50, 80, 20), "Missile Max Speed");
		
		m.MaxSpeed = float.Parse (GUI.TextField (new Rect (90, 50, 40, 20), m.MaxSpeed.ToString ()));
		
		if (GUI.Button (new Rect (10, 70, 20, 20), "+"))
			m.MaxSpeed += 10;
		if (GUI.Button (new Rect (40, 70, 20, 20), "-"))
			m.MaxSpeed -= 10;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		//m.MaxSpeed = speed;
		
		
		if (playerCraft.PrimaryTarget != null) { //check that target still exists
						
//			if (m == null)
//				m.Position = basestation.transform.position;
//			else
//				m.Position = fox2.transform.position;

			m.TargetPosition = playerCraft.PrimaryTarget.transform.position;
			
			m.TargetVelocityVector = m.CalculateVelocityVector(m.oldTargetPosition, m.TargetPosition, Time.deltaTime); // who should do this the players craft or the missile???
			
			if (Input.GetKey (KeyCode.F)) {			            		            			
						
				launch = true;			
			
			}

			if (launch && !launched) {
				
				launch = false;
				
				// my missile
				m.EntityObj = (GameObject)Instantiate (missile, playerCraft.Position, playerCraft.Rotation);
				
				// calculate the intercept vector which is the target and missile will collide at time t based on missiles maxspeed
				Vector3 interceptVector = m.CalculateInterceptVector(m.TargetPosition, m.TargetVelocityVector, m.Position, m.MaxSpeed);
				
				// calculate the velocity vector required for the missile to travel that will reach intercept
				Vector3 missileVelocityVectorToIntercept = m.PlotCourse(interceptVector);
				
				to = missileVelocityVectorToIntercept;
				
				something = m.CalculateInterceptVector(m.TargetPosition, targVelocity, m.Position, m.MaxSpeed); // for debug only!!!
				
				// create a rigid body for our missile
				m.EntityObj.AddComponent<Rigidbody> ();
				// create a sphere collider for our missile
				m.EntityObj.AddComponent<SphereCollider>();				                 
				
				SphereCollider sc = (SphereCollider)m.EntityObj.collider;
				
				sc.radius = 0.5f; //set its intital det range
				
				m.EntityObj.rigidbody.useGravity = false;
				m.EntityObj.rigidbody.angularDrag = 0;
				m.EntityObj.rigidbody.mass = 1;											
				
				launched = true;
			}
								
			if (launched && m.EntityObj != null)
			{
				//recalculate
				
				Vector3 interceptVector = m.CalculateInterceptVector(m.TargetPosition, m.TargetVelocityVector, m.Position, m.MaxSpeed);
				
				Vector3 missileVelocityVectorToIntercept = m.PlotCourse(interceptVector);
				
				
				// Check if path to intercept is still viable...
				// if not we will check if in detonation range
				// if not in detonation range we will continue on old velocity and hope for better intercept chance
				if (interceptVector == Vector3.zero)
				{
					
					// the path is not viable so lets check if missile is in detonation range of target					
					if (m.InDetonationRange())
					{
						
						
						SphereCollider myCollider = m.EntityObj.transform.GetComponent<SphereCollider>();
						myCollider.radius = m.detonationRange;
						
						
					}				
					
				}				
				else
				{	
					// This path is viable so update missile
					to = missileVelocityVectorToIntercept; 
				}	
				
				m.EntityObj.transform.forward = Vector3.Normalize(to);
				m.EntityObj.transform.position += to * Time.deltaTime;
				
				// Debug onlu
				Debug.DrawLine(playerCraft.Position, something, Color.blue, 10, false); 
				Debug.DrawLine(Vector3.zero, something, Color.white, 10, false);
			}

			
			m.oldTargetPosition = m.TargetPosition;		
		}
	}	
	
}
