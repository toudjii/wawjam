using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update

    public Rigidbody rb;
    public Vector3 rot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.transform.Rotate(rot);
    }
}
