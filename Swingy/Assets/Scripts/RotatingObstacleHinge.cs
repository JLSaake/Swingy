using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObstacleHinge : Obstacle
{
    HingeJoint2D hinge;

    [Range(0.0f, 0.95f)]
    public float hingePosition = 0f;

    float torque;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLength(float newLength)
    {
        gameObject.transform.GetChild(0).localScale = new Vector2(1f, newLength);
    }

    public float GetLength()
    {
        return gameObject.transform.GetChild(0).localScale.y;
    }

    public void SetTorque(float newTorque)
    {
        torque = newTorque;
        gameObject.transform.GetChild(0).gameObject.GetComponent<RotateObstacle>().torque = this.torque * Mathf.Pow(hingePosition + 1.0f, 2.0f);
    }

    public float GetTorque()
    {
        return gameObject.transform.GetChild(0).gameObject.GetComponent<RotateObstacle>().torque;
    }

    public void SetHingePoint(float newHingePoint)
    {
        if(!hinge)  hinge = GetComponent<HingeJoint2D>();
        hingePosition = newHingePoint;
        hinge.connectedAnchor = new Vector2(0.0f, hingePosition * 2.0f);
    }
}
