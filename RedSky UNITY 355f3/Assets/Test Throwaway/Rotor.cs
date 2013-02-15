using UnityEngine;
using System.Collections;

public class Rotor : MonoBehaviour {
	
	Vector3 verticalaxis;
	float speedToRotate;
	
	// Use this for initialization
	void Start () {
		speedToRotate = 2000f;
	}
	
	// Update is called once per frame
	void Update () {
		
		verticalaxis = Vector3.Cross(transform.up, transform.right);
		
		transform.RotateAround(this.transform.position, verticalaxis, speedToRotate * Time.deltaTime); 
	}
}
