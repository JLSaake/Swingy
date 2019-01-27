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
    public GameObject rotatingObstacle;

    // [Range(0.5f, 2.0f)]
    public float ropeLengthLow;
    public float ropeLengthHigh;
    public float averageRopeSpacing;
    [Range(0.01f, 0.5f)]
    public float ropeSpacingVariation;

    // Level parameters
    int maxHeight;
    float lastX, lastY, startY;
    HeightIncrement heightIncrement;
    float obsSpawn;

    // Internal state management per level
    int height;
    float climbChance;
    float maxClimbChance;
    System.Random rand;

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();

        // Level 1 config
        // generateLevel(15, 16, new Vector2(0, 2.81f), HeightIncrement.Decrease, 0f);

        // Level 2 config
        generateLevel(16, 35, new Vector2(0, 2.81f), HeightIncrement.Linear, 0.6f);
        
        // for(int t=0; t<50; ++t) Instantiate(rope, ArcCalculator.maxPosAtTime(t/5), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Obstacle spawn rate- a float between 0 and 1 representing obstacle spawn chance after each rope.
    void generateLevel(int numRopes, int maxHeight, Vector2 startPos, HeightIncrement heightIncrement, float obstacleSpawnChance)
    {
        this.maxHeight = maxHeight;
        this.heightIncrement = heightIncrement;
        this.lastX = startPos.x;
        this.lastY = startPos.y;
        this.startY = startPos.y;
        this.obsSpawn = obstacleSpawnChance;

        // Reset internal level state
        height = 0;
        climbChance = 0.55f;

        // Generate level
        for(int i=0; i<numRopes; ++i)
        {
            nextRope();
        }
    }

    void nextRope()
    {
        float spacingChange = (float)(rand.NextDouble() + rand.NextDouble())*ropeSpacingVariation + 1f - ropeSpacingVariation;
        // Debug.Log(spacingChange);
        float xPos = lastX + averageRopeSpacing * spacingChange;
        
        bool climb = false;

        if(height < maxHeight)
        {
            climb = rand.NextDouble() < climbChance;
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
        GameObject tempRope;
        tempRope = Instantiate(rope, new Vector2(xPos, 1.5f * ((float)height) + startY), Quaternion.identity);

        if(rand.NextDouble() < obsSpawn)
        {
            float obstPos;
            if(climb)   obstPos = -5f;
            else    obstPos = 2.5f;
            GameObject tempObstacle = Instantiate(rotatingObstacle, new Vector2((xPos + lastX)/2.0f, (float)height + lastY + obstPos), Quaternion.identity);
            var tempScript = tempObstacle.GetComponent<RotatingObstacleHinge>(); 
            //tempScript.SetLength(Mathf.Min(averageRopeSpacing * spacingChange/5.8f, Vector2.Distance(tempObstacle.transform.position, tempRope.transform.position)));
            tempScript.SetLength(Vector2.Distance(tempObstacle.transform.position, new Vector2(lastX, lastY))/2f-2.5f );
            tempScript.SetTorque(10f * (-12f + (float)rand.NextDouble()));
            tempScript.SetHingePoint(0.05f);
        }

        lastX = xPos;
        lastY = 1.5f * ((float)height) + startY;
        // tempRope.transform.GetChild(0).gameObject.GetComponent<Rope>().SetLength(spacingChange + 1.1f);
        // Debug.Log(height + " " + climbChance);
        // return new Vector2(xPos, 1.5f * ((float)height) + lastY);
    }

}
