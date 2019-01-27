using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum HeightIncrementType
{
    Linear,
    Decrease
};

public enum HeightGain
{
    Trivial,
    Slight
}

public class Procedural : MonoBehaviour
{

    public GameObject rope;
    public GameObject rotatingObstacle;
    public GameObject blocker;
    public GameObject spike;

    // [Range(0.5f, 2.0f)]
    public float ropeLengthLow;
    public float ropeLengthHigh;
    public float averageRopeSpacing;
    [Range(0.01f, 0.5f)]
    public float ropeSpacingVariation;

    // Level parameters
    int maxHeight;
    float lastX, lastY, startY;
    HeightIncrementType heightIncrement;
    HeightGain heightGain;
    float obsSpawn;

    public int level1Ropes = 15;
    public int level2Ropes = 16;
    public int level3Ropes = 17;
    private static int maxRopes;

    // Internal state management per level
    int height;
    float climbChance;
    float maxClimbChance;
    System.Random rand;

    // Start is called before the first frame update
    void Start()
    {
        maxRopes = level1Ropes + level2Ropes + level3Ropes;
        rand = new System.Random();

        // Level 1 config
        generateLevel(level1Ropes, 16, new Vector2(0, 2.81f), HeightIncrementType.Decrease, HeightGain.Trivial, 0f);

        // Level 2 config
        // generateLevel(level2Ropes, 32, new Vector2(0, 2.81f), HeightIncrementType.Linear, HeightGain.Slight, 0.64f);
        
        // for(int t=0; t<50; ++t) Instantiate(rope, ArcCalculator.maxPosAtTime(t/5), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Obstacle spawn rate- a float between 0 and 1 representing obstacle spawn chance after each rope.
    void generateLevel(int numRopes, int maxHeight, Vector2 startPos, HeightIncrementType heightIncrement, HeightGain heightGain, float obstacleSpawnChance)
    {
        this.maxHeight = maxHeight;
        this.heightIncrement = heightIncrement;
        this.heightGain = heightGain;
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
        float yPos = lastY - 0.3f + (float)rand.NextDouble() * 0.8f;
        
        bool climb = false;

        if(height < maxHeight)
        {
            climb = rand.NextDouble() < climbChance;
            if(climb)
            {
                switch(heightGain)
                {
                    case HeightGain.Trivial:
                        yPos += 1.3f + (float)rand.NextDouble() * 0.4f;
                        break;
                    case HeightGain.Slight:
                        yPos += 1.6f + (float)rand.NextDouble() * 0.8f;
                        break;
                }
                climbChance = 0.04f + 0.88f * Mathf.Pow((maxHeight - height) / (float)maxHeight, 1.7f);
            }
            else
            {
                switch(heightIncrement)
                {
                    case HeightIncrementType.Linear:
                        climbChance += 0.38f;
                        break;
                    case HeightIncrementType.Decrease:
                        climbChance += 0.45f * Mathf.Pow(0.89f * (maxHeight - height) / (float)maxHeight, 1.5f);
                        break;
                    // default:
                    //     Debug.Log("");
                }
                
            }

        }
        GameObject tempRope;
        tempRope = Instantiate(rope, new Vector2(xPos, yPos), Quaternion.identity);

        if(rand.NextDouble() < obsSpawn)
        {
            // Create an obstacle
            double obsChoice = rand.NextDouble();

            if(obsChoice < 0.2)
            {
                float obstPos;
                if(climb)   obstPos = -4.95f - (float)rand.NextDouble() * 0.9f;
                else    obstPos = 2.65f + (float)rand.NextDouble() * 0.65f;
                GameObject tempObstacle = Instantiate(rotatingObstacle, new Vector2((xPos + lastX)/2.0f, yPos + obstPos), Quaternion.identity);
                var tempScript = tempObstacle.GetComponent<RotatingObstacleHinge>(); 
                //tempScript.SetLength(Mathf.Min(averageRopeSpacing * spacingChange/5.8f, Vector2.Distance(tempObstacle.transform.position, tempRope.transform.position)));
                float lengthOffset;
                if(climb)   lengthOffset = -2.5f;
                else        lengthOffset = -1.5f;
                tempScript.SetLength(Vector2.Distance(tempObstacle.transform.position, new Vector2(lastX, lastY))/2f + lengthOffset );
                tempScript.SetTorque(10f * (-12f + (float)rand.NextDouble()));
                tempScript.SetHingePoint(0.05f);
            }
            else if(obsChoice < 0.62 && (xPos - lastX) > 9.8f)
            {
                float dx = (float)(rand.NextDouble()) * 0.2f + 0.4f;   // Between 0.4 and 0.6
                float obstacleX = (dx*xPos + (1f-dx)*lastX);
                float dy = Mathf.Pow((float)(rand.NextDouble() + 0.5) / 2f, 2f) + 0.1875f;   // Between 0.25 and 0.75, weighted towards edges
                float obstacleY;
                if(climb)   obstacleY = lastY - 4.4f + (float)rand.NextDouble() * 2.4f;
                else
                {
                    obstacleY = (dy * yPos + (1f-dy) * lastY) - 8.5f + (Mathf.Pow(dy + 0.5f, 1.5f) - 0.5f) * 8.5f;
                    if(obstacleX - lastX < 5.5f)
                    {
                        obstacleY = lastY - 3f + (float)rand.NextDouble() * 2f;
                        obstacleX += 0.5f;
                    }
                }
                
                GameObject tempBlockObstacle = Instantiate(blocker, new Vector2(obstacleX, obstacleY), Quaternion.identity);
                tempBlockObstacle.GetComponent<Blocker>().SetAnimationStart((float)rand.NextDouble() * 1.6f);
            }
            else
            {
                float dx = (float)(rand.NextDouble()) * 0.2f + 0.4f;   // Between 0.4 and 0.6
                float obstacleX = (dx*xPos + (1f-dx)*lastX);
                float obstacleY = (lastY + yPos) / 2f - 6.2f + (float)rand.NextDouble() * 1.5f;
                
                GameObject tempSpike = Instantiate(spike, new Vector2(obstacleX, obstacleY), Quaternion.identity);
                if(dx < 0.47 && rand.NextDouble() < 0.4) Instantiate(spike, new Vector2(obstacleX + 1.3f, obstacleY), Quaternion.identity);
            }

        }

        lastX = xPos;
        lastY = yPos;
        // tempRope.transform.GetChild(0).gameObject.GetComponent<Rope>().SetLength(spacingChange + 1.1f);
        // Debug.Log(height + " " + climbChance);
        // return new Vector2(xPos, 1.5f * ((float)height) + lastY);
    }

    public static int GetMaxRopes()
    {
        return maxRopes;
    }

}
