using UnityEngine;
using System.Collections;

public class Reply : MonoBehaviour
{
    int TTL = 30;    
    float tick;
    Vector3 scale;    
    public string message;
    public Material transparent;

    // Use this for initialization
    void Start()
    {
        
        tick = 1;
        scale = new Vector3(150, 150, 150);
                
        gameObject.AddComponent<Rigidbody>();
        gameObject.AddComponent<SphereCollider>();

        

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.drag = 0f;
        rb.angularDrag = 0f;

        SphereCollider p = gameObject.GetComponent<SphereCollider>();

        p.isTrigger = true;

        Renderer r = gameObject.GetComponent<Renderer>();

        //r.enabled = false;

        r.material = transparent;

        gameObject.name = "" + message + "_reply";
    }

    // Update is called once per frame
    void Update()
    {
        

        TTL--;
                
        gameObject.transform.localScale = scale * tick;
        tick++;

        if (TTL <= 0)
            Destroy(gameObject);

    }

   
}


