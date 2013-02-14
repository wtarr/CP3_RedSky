using UnityEngine;
using System.Collections;

public class MissileController : MonoBehaviour {

    Missile m;
	
    // Use this for initialization
	void Start () {
        m = new Missile();
        //m.TargetPosition = new Vector3(1, 2, 3);

        if (m == null)
            Debug.Log("missile is null");
        else
        {
            Debug.Log("Missile is live");
            //Debug.Log(m.TargetPosition);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
