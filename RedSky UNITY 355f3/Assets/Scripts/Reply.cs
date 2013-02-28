using UnityEngine;
using System.Collections;

public class Reply : MonoBehaviour {
	
	bool reply;
	float tick;
	Vector3 scale;
	GameObject replyToSender;
	string message;
	public Material transparent;
	
	// Use this for initialization
	void Start () {
		reply = false;
		tick = 1;
		scale = new Vector3(150, 150, 150);
		message = string.Empty;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (reply)
		{
				reply = false;
			
				replyToSender = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				
				replyToSender.AddComponent<Rigidbody>();
											
				replyToSender.name = message;
			
				replyToSender.transform.position = transform.position;
				
				Rigidbody rb = replyToSender.GetComponent<Rigidbody>();
				
				rb.useGravity = false;
				rb.drag = 0f;
				rb.angularDrag = 0f;				
				
				SphereCollider p = replyToSender.GetComponent<SphereCollider>();
				
				p.isTrigger = true;
				
				Renderer r = replyToSender.GetComponent<Renderer>();
				
				r.material = transparent;
			
				//r.enabled = false;
				
				//p.radius = 10;
				
				
		}
		
		if (replyToSender != null)
		{			
			replyToSender.transform.position = transform.position;
			replyToSender.transform.localScale= scale * tick;
			tick++;			
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name.Contains("ping") && !other.gameObject.name.Contains("reply"))
		{
			Debug.Log("Pinged");	
			
			if (message == string.Empty)
				message = "replyTo_" + other.gameObject.name; // + " to " + other.gameObject.name + " reliers coords " + gameObject.transform.position.ToString();
			
			reply = true;
		
			Destroy(other.gameObject);
		}
		
		
		
		
		
		
	}
}
