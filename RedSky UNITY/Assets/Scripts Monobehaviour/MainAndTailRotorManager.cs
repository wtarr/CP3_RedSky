using UnityEngine;
using System.Collections;

public class MainAndTailRotorManager : MonoBehaviour {

    public GameObject mainRotor, tailRotor;

    Vector3 verticalAxis, horizontalAxis;
    float speedToRotate;

    // Use this for initialization
    void Start()
    {
        speedToRotate = 2000f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        verticalAxis = Vector3.Cross(transform.up, transform.right);
        horizontalAxis = Vector3.Cross(transform.up, transform.forward);
        mainRotor.transform.RotateAround(mainRotor.transform.position, verticalAxis, speedToRotate * Time.deltaTime);
        tailRotor.transform.RotateAround(tailRotor.transform.position, transform.up, speedToRotate * Time.deltaTime);
    }
}
