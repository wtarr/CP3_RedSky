using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TestMissile : MonoBehaviour {
	
	const float MISSILEMAXSPEED = 20f;


    GameObject target;

    Vector3 targetVelocity;

    List<Vector3> velocityList;

    Vector3 targetPositionOld, targetPositionNew;

    float start, finish, timeMeasuring;

    float targetSpeed;


	// Use this for initialization
	void Start () {


        target = GameObject.Find("target");

        velocityList = new List<Vector3>();
		targetVelocity = Vector3.zero;
		
		targetPositionNew = target.transform.position;
		targetPositionOld = targetPositionNew;

	}
	
	// Update is called once per frame
	void Update () {

        start = Time.realtimeSinceStartup;

        targetPositionNew = target.transform.position; // assume radar ping

        if (targetPositionOld != targetPositionNew)
            targetVelocity = targetPositionNew - targetPositionOld;
		
		if (targetVelocity != Vector3.zero)
        	velocityList.Add(targetVelocity);

        if (velocityList.Count >= 2)
        {
            //form a overall velocity by adding them up to form one velocity
            foreach (var item in velocityList)
            {
                targetVelocity += item;
            }

            finish = Time.realtimeSinceStartup;

            timeMeasuring = finish - start;

            targetSpeed = Vector3.Magnitude(targetVelocity) / timeMeasuring;

            velocityList.Clear();
        }






		//transform.position += transform.forward * SPEED * Time.deltaTime;

        targetPositionOld = targetPositionNew;
	
	}

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), String.Format("Target m/s: {0:0.00}", targetSpeed));
    }
}
