using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObstacle : MonoBehaviour
{
    public float torque = 1.5f;
    Rigidbody2D rb;

    private float stopTime = 0f;
    private const float STOP_SWITCH_TIME = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(rb.angularVelocity) < 0.1f)  stopTime += Time.deltaTime;
        if(stopTime > STOP_SWITCH_TIME)
        {
            stopTime = 0f;
            torque *= -1f;
        }
        rb.AddTorque(torque);
    }
}
