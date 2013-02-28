using UnityEngine;
using System.Collections;

public class Ping : MonoBehaviour {
	
	float tick = 1;
	Vector3 scale;
	GameObject ping;
	bool sent;
	public Material transparent;
	
	// Use this for initialization
	void Start () {
		sent = false;
		scale = new Vector3(10, 10, 10);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (ping != null)
			ping.transform.position = transform.position;
		
		if (Input.GetKey(KeyCode.F))
		{
			
			if (!sent)
			{
				ping = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				
				
				
				ping.AddComponent<Rigidbody>();
				
				//ping.AddComponent<SphereCollider>();
				
				ping.name = "ping" + Time.timeSinceLevelLoad.ToString(); 
				
				//ping.tag = 
			
				ping.transform.position = transform.position;
				
				Rigidbody rb = ping.GetComponent<Rigidbody>();
				
				rb.useGravity = false;
				rb.drag = 0f;
				rb.angularDrag = 0f;
				
				
				SphereCollider p = ping.GetComponent<SphereCollider>();
				
				p.isTrigger = true;
				
				Renderer r = ping.GetComponent<Renderer>();
				
				r.renderer.material = transparent;
				
				//r.enabled = false;
				
				//p.radius = 10;
				
				sent = true;
			}
			
		}
		
		
		if (ping != null)
		{
			ping.transform.localScale= scale * tick;
			tick++;
		
		}
		
	}
	
	void OnTriggerEnter(Collider other)
	{
		
		if (other.gameObject.name.Contains("reply"))
		{
			Debug.Log("Received");
			//Debug.Log(other.gameObject.name);
			//Debug.Log(other.transform.position);
		
			Destroy(other.gameObject);
		}
	}
}
