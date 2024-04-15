using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Cube : MonoBehaviour
{

    public float thrust = 1.0f;
    public float forward = 1.0f;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(forward, 0, thrust, ForceMode.Impulse);
    }

}

