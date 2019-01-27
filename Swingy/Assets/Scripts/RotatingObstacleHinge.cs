using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObstacleHinge : MonoBehaviour
{
    HingeJoint2D hinge;
    [Range(0.0f, 0.95f)]
    public float obstaclePosition;

    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        hinge.connectedAnchor = new Vector2(0.0f, obstaclePosition * 2.0f);
        gameObject.transform.GetChild(0).gameObject.GetComponent<RotateObstacle>().torque *= Mathf.Pow(obstaclePosition + 1.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
