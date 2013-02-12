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
	Vector3 newTarPos, oldTarPos;
	bool launch = false, launched = false;
	
	
	// Use this for initialization
	void Start ()
	{
		
		m = new Missile ();
		
		speed = 10f;
		
		m.MaxSpeed = speed;		
		
		
		target = GameObject.Find ("target");
		
		basestation = GameObject.Find("missilepod");
		
	}
	
	void OnGUI ()
	{
		GUI.Label (new Rect (10, 10, 40, 20), "Speed");
		GUI.Label (new Rect (150, 10, 140, 20), "Speed: " + m.MaxSpeed.ToString());
		speed = float.Parse (GUI.TextField (new Rect (50, 10, 40, 20), speed.ToString ()));
		
		if (GUI.Button (new Rect (10, 30, 20, 20), "+"))
			speed += 10;
		if (GUI.Button (new Rect (40, 30, 20, 20), "-"))
			speed -= 10;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		m.MaxSpeed = speed;
		
		
		if (GameObject.Find ("target")) { //check that target still exists
			
			m.newMissilePosition = transform.position;

			m.TargetPosition = target.transform.position;
			
			m.newTargetPosition = m.TargetPosition;

			m.CalculateTargetsVelocityandSpeed ();


			if (Input.GetKey (KeyCode.Space)) {
			            		            			
						
				launch = true;
				
			
			}

			if (launch && !launched) {
				
				launch = false;
				launched = true;
				
				fox2 = (GameObject)Instantiate (missile, basestation.transform.position, transform.rotation);
					
				Vector3 v = m.CalculateInterceptVector ();
				to = v;
				
				
			
				fox2.AddComponent<Rigidbody> ();
							
				
				
				fox2.AddComponent<SphereCollider>();
				
				
								
				
				
				
				SphereCollider sc = (SphereCollider)fox2.collider;
				
				sc.radius = 0.5f;
				
				fox2.rigidbody.useGravity = false;
				fox2.rigidbody.angularDrag = 0;
				fox2.rigidbody.mass = 1;
			
				fox2.transform.forward = v;
			
				
			}
								
			if (launched)
				fox2.transform.position += fox2.transform.forward * m.MaxSpeed * Time.deltaTime;
		
//		if( Input.GetMouseButton(0))
//		{
//			
//			Instantiate(obj, transform.position, transform.rotation);
//		}
        
			///Debug.DrawLine(basestation.transform.position, to);
			
			
			//Debug.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 5, 0), Color.blue, 3, false);
			if (launched)
				Debug.DrawLine(m.newMissilePosition, to * 100, Color.blue, 10, false);
			
			
			m.oldTargetPosition = m.newTargetPosition;		
		}
	}	
	
}
