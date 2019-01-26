using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Procedural : MonoBehaviour
{

    // gIVE mE rOPE
    public GameObject rope;

    // Level parameters
    int maxHeight;

    // Internal state management per level
    int height;
    float climbChance;
    float maxClimbChance;
    System.Random rand;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        configureParams(16);
        rand = new System.Random();
        for(int i=0; i<40; ++i) Instantiate(rope, new Vector2((float)i, (float)nextRope()), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void configureParams(int maxHeight)
    {
        this.maxHeight = maxHeight;

        // Reset internal level state
        height = 0;
        climbChance = 0.30f;
    }

    int nextRope()
    {
        if(height < maxHeight)
        {
            bool climb = rand.NextDouble() < climbChance;
            if(climb)
            {
                ++height;
                climbChance = 0.05f + ;
            }
            else
            {
                climbChance += 0.35f * (maxHeight - height) / maxHeight;
            }
        }
        // Debug.Log("hi" + climbChance);
        return height;
    }

}
