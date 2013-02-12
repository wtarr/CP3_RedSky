using UnityEngine;
using System.Collections;

public class TestTarget : MonoBehaviour {

    public const float SPEED = 15f;
	public GameObject exp;
	Vector3 start;
	
    // Use this for initialization
    void Start()
    {
		start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position += transform.forward * SPEED * Time.deltaTime;
		
		Debug.DrawLine(start, transform.position, Color.blue, 10, false);

    }
	
	void OnCollisionEnter(Collision collision)
	{
		
		Debug.Log("Triggered");
		
		if (collision.gameObject.name == "missile(Clone)" || collision.gameObject.name == "missilefox2(Clone)" )
		{
			Instantiate(exp, transform.position, transform.rotation);
			
			Destroy(collision.gameObject);
			Destroy(GameObject.Find("target"));
			
		}
	}
	
	
}
