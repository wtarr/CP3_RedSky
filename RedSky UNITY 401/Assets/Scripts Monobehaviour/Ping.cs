using UnityEngine;
using System.Collections;

public class Ping : MonoBehaviour
{

    float tick = 1;
    int TTL = 15;
    Vector3 scale;    
    public Material transparent;

    // Use this for initialization
    void Start()
    {
        gameObject.name = "ping" + Time.timeSinceLevelLoad.ToString();
        
        gameObject.AddComponent<Rigidbody>();

        gameObject.AddComponent<SphereCollider>();

        gameObject.transform.position = transform.position;

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.drag = 0f;
        rb.angularDrag = 0f;

        SphereCollider p = gameObject.GetComponent<SphereCollider>();

        p.isTrigger = true;

        Renderer r = gameObject.GetComponent<Renderer>();

        r.renderer.material = transparent;
        
        r.enabled = false;

        //p.radius = 10;
       
        scale = new Vector3(200, 200, 200);
    }

    // Update is called once per frame
    void Update()
    {
        TTL--;

        if (TTL <= 0)
        {
            Destroy(gameObject);
        }

        gameObject.transform.localScale = scale * tick;
        
        tick++;

    }


}
