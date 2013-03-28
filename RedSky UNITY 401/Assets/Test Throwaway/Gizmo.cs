using UnityEngine;
using System.Collections;

public class Gizmo : MonoBehaviour {

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Light Gizmo.tiff");
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
