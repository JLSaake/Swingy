using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum HeightIncrement
{
    Linear,
    Decrease
};

public class Procedural : MonoBehaviour
{

    // gIVE mE rOPE
    public GameObject rope;
    // [Range(0.5f, 2.0f)]
    public float ropeLengthLow;
    public float ropeLengthHigh;
    public float averageRopeSpacing;
    [Range(0.01f, 0.5f)]
    public float ropeSpacingVariation;

    // Level parameters
    int maxHeight;
    float lastX, lastY;
    HeightIncrement heightIncrement;

    // Internal state management per level
    int height;
    float climbChance;
    float maxClimbChance;
    System.Random rand;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        configureParams(16, new Vector2(0,0), HeightIncrement.Decrease);
        rand = new System.Random();
        for(int i=0; i<40; ++i) Instantiate(rope, nextRope(), Quaternion.identity);
        // for(int t=0; t<50; ++t) Instantiate(rope, ArcCalculator.maxPosAtTime(t/5), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void configureParams(int maxHeight, Vector2 startPos, HeightIncrement heightIncrement)
    {
        this.maxHeight = maxHeight;
        this.heightIncrement = heightIncrement;
        this.lastX = startPos.x;
        this.lastY = startPos.y;

        // Reset internal level state
        height = 0;
        climbChance = 0.30f;
    }

    Vector2 nextRope()
    {
        float xPos = lastX + averageRopeSpacing * ((float)(rand.NextDouble() + rand.NextDouble())*ropeSpacingVariation + 1f - ropeSpacingVariation);
        lastX = xPos;
        if(height < maxHeight)
        {
            bool climb = rand.NextDouble() < climbChance;
            if(climb)
            {
                ++height;
                climbChance = 0.05f;
            }
            else
            {
                switch(heightIncrement)
                {
                    case HeightIncrement.Linear:
                        climbChance += 0.30f;
                        break;
                    case HeightIncrement.Decrease:
                        climbChance += 0.45f * (maxHeight - height) / maxHeight;
                        break;
                    default:
                        Debug.Log("");
                }
                
            }
        }
        // Debug.Log("hi" + climbChance);
        return new Vector2(xPos, (float)height);
    }

}
