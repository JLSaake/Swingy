using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcCalculator : MonoBehaviour
{

    static float maxVelocity = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector2 posAtTime(float time, float angle)
    {
        return new Vector2(0.0f, 0.0f);
        // Angle is a degree value between 0 and 90. (0 is right, 90 is up)
    }

    public static Vector2 maxPosAtTime(float time)
    {
        float ScaledVelocity = maxVelocity * 0.7071f;
        float vx = maxVelocity / 2.0f, vy = vx;
        return new Vector2(vx * time, -0.681f * Mathf.Pow(time, 2) + vy * time);
    }
}
