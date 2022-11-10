using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;
using UnityEngine;


public class test : MonoBehaviour
{
    // Start is called before the first frame update

    public Rigidbody rb;
    public Vector3 rot;
    public VisualEffect boostvfx;
    public float boostvfyspeed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        boostvfx.SetFloat("ParticleRate", rot.magnitude* boostvfyspeed);
        rb.transform.Rotate(rot);
    }
}
