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
	Vector3 newRelTarPos, oldRelTarPos, relTargVelocity, something;
	bool launch = false, launched = false;
	
	
	// Use this for initialization
	void Start ()
	{
		
		m = new Missile ();
		
		speed = 60f;
		
		m.MaxSpeed = speed;		
		
		
		target = GameObject.Find ("target");
		
		
		
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
			
			m.newMissilePosition = basestation.transform.position;

			m.TargetPosition = target.transform.position;
			
			newRelTarPos = basestation.transform.InverseTransformDirection(target.transform.position - basestation.transform.position);
			
			relTargVelocity = m.CalculateVelocityVector(oldRelTarPos, newRelTarPos, Time.deltaTime);
			
			//basestation.transform.LookAt(target.transform);
			

			if (Input.GetKey (KeyCode.F)) {
			            		            			
						
				launch = true;
				
			
			}

			if (launch && !launched) {
				
				launch = false;
				launched = true;
				
				fox2 = (GameObject)Instantiate (missile, basestation.transform.position, transform.rotation);
					
				Vector3 v = m.CalculateInterceptVector(newRelTarPos, relTargVelocity, m.MaxSpeed);
						
				to = v;
				
				something = m.CalculateInterceptVector(newRelTarPos, relTargVelocity, m.newMissilePosition, m.MaxSpeed);
			
				fox2.AddComponent<Rigidbody> ();
							
				
				
				fox2.AddComponent<SphereCollider>();
				                 
				
				SphereCollider sc = (SphereCollider)fox2.collider;
				
				sc.radius = 0.5f;
				
				fox2.rigidbody.useGravity = false;
				fox2.rigidbody.angularDrag = 0;
				fox2.rigidbody.mass = 1;
			
				fox2.transform.forward = v;
			
				
			}
								
			if (launched && fox2 != null)
			{
				fox2.transform.forward = Vector3.Normalize(to);
				fox2.transform.position += to * Time.deltaTime;
			}

			if (launched)
			{
				Debug.DrawLine(basestation.transform.position, to * 1000, Color.blue, 10, false);
				Debug.DrawLine(Vector3.zero, something, Color.white, 10, false);
			
			}
			oldRelTarPos = newRelTarPos;		
		}
	}	
	
}
