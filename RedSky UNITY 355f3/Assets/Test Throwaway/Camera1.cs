using UnityEngine;
using System.Collections;

public class Camera1 : MonoBehaviour {
	
	GameObject target;
	
	// Use this for initialization
	void Start () {
		target = GameObject.Find("target");
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null)
			transform.LookAt(target.transform);
	}
}
