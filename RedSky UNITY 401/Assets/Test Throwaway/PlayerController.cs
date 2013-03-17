using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    float speed = 5f;
    float rotationSpeed = 0.05f;
    Vector3 velocity;

	// Use this for initialization
	void Start () {

        if (!networkView.isMine)
        {
            GetComponentInChildren<Camera>().enabled = false;
        }
        
    }
	
	// Update is called once per frame
	void Update () {

        velocity = speed * transform.forward;
        

        if (Input.GetKey(KeyCode.UpArrow))
            transform.position += velocity * Time.deltaTime;


        if (Input.GetKey(KeyCode.LeftArrow))
            transform.RotateAround(transform.up, -1 * rotationSpeed);

        if (Input.GetKey(KeyCode.RightArrow))
            transform.RotateAround(transform.up, rotationSpeed);
	}
}
