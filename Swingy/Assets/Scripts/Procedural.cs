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
        rand = new System.Random();
        generateLevel(20, 16, new Vector2(0, 2.81f), HeightIncrement.Decrease);
        
        // for(int t=0; t<50; ++t) Instantiate(rope, ArcCalculator.maxPosAtTime(t/5), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void generateLevel(int numRopes, int maxHeight, Vector2 startPos, HeightIncrement heightIncrement)
    {
        this.maxHeight = maxHeight;
        this.heightIncrement = heightIncrement;
        this.lastX = startPos.x;
        this.lastY = startPos.y;

        // Reset internal level state
        height = 0;
        climbChance = 0.55f;

        // Generate level
        for(int i=0; i<numRopes; ++i) Instantiate(rope, nextRope(), Quaternion.identity);
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
                climbChance = 0.04f + 0.88f * Mathf.Pow((maxHeight - height) / (float)maxHeight, 1.7f);
            }
            else
            {
                switch(heightIncrement)
                {
                    case HeightIncrement.Linear:
                        climbChance += 0.38f;
                        break;
                    case HeightIncrement.Decrease:
                        climbChance += 0.45f * Mathf.Pow(0.89f * (maxHeight - height) / (float)maxHeight, 1.5f);
                        break;
                    // default:
                    //     Debug.Log("");
                }
                
            }
        }
        Debug.Log(height + " " + climbChance);
        return new Vector2(xPos, 1.5f * ((float)height) + lastY);
    }

}
