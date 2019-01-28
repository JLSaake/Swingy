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
    Slight,
    Medium,
    Steep
}

public class Procedural : MonoBehaviour
{

    public Transform firstRope;
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

    Vector2 nextStartPos;

    public static int level1Ropes = 5;
    public static int level2Ropes = 10;
    public static int level3Ropes = 10;
    private static int maxRopes;
    private int currentID = 1;

    // Internal state management per level
    int height;
    float climbChance;
    float maxClimbChance;
    float backtrackChance;
    System.Random rand;
    bool jumpBack;

    // Start is called before the first frame update
    void Start()
    {
        maxRopes = level1Ropes + level2Ropes + level3Ropes;
        rand = new System.Random();
        
        buildLevel(1);
        buildLevel(2);
        buildLevel(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buildLevel(int levelNum)
    {
        switch(levelNum)
        {
            case 1:
                generateLevel(level1Ropes, 16, firstRope.transform.position, HeightIncrementType.Decrease, HeightGain.Slight, 0f, 0.0f, true);
                break;
            case 2:
                generateLevel(level2Ropes, 32, nextStartPos, HeightIncrementType.Linear, HeightGain.Medium, 0.44f, 0.19f, true);
                break;
            case 3:
                generateLevel(level3Ropes, 32, nextStartPos, HeightIncrementType.Linear, HeightGain.Steep, 0.67f, 0.36f, false);
                break;
        }
    }

    // Obstacle spawn rate- a float between 0 and 1 representing obstacle spawn chance after each rope.
    void generateLevel(int numRopes, int maxHeight, Vector2 startPos, HeightIncrementType heightIncrement, HeightGain heightGain, float obstacleSpawnChance,
        float backtrackChance, bool genStart)
    {
        this.maxHeight = maxHeight;
        this.heightIncrement = heightIncrement;
        this.heightGain = heightGain;
        this.lastX = startPos.x;
        this.lastY = startPos.y;
        this.startY = startPos.y;
        this.obsSpawn = obstacleSpawnChance;
        this.backtrackChance = backtrackChance;

        // Reset internal level state
        height = 0;
        climbChance = 0.55f;

        // Generate level
        for(int i=0; i<numRopes; ++i)
        {
            jumpBack = i<numRopes-4 && (float)rand.NextDouble() < backtrackChance;
            nextRope();
        }

        if(genStart)
        {
            obsSpawn = 0f;
            jumpBack = false;
            nextStartPos = nextRope();
        }
    }

    Vector2 nextRope()
    {
        float spacingChange = (float)(rand.NextDouble() + rand.NextDouble())*ropeSpacingVariation + 1f - ropeSpacingVariation;
        // Debug.Log(spacingChange);
        float xPos = lastX + ((jumpBack) ? -0.71f : 1f) * averageRopeSpacing * spacingChange;
        float yPos = lastY - 0.3f + (float)rand.NextDouble() * 0.8f;
        if(jumpBack)   yPos = lastY + 5.0f - 1.1f * spacingChange;
        else if(heightGain == HeightGain.Steep) yPos += 1.0f;
        
        bool climb = false;

        if(height < maxHeight)
        {
            climb = jumpBack || rand.NextDouble() < climbChance;
            if(climb)
            {
                switch(heightGain)
                {
                    case HeightGain.Trivial:
                        yPos += 1.2f + (float)rand.NextDouble() * 0.4f;
                        break;
                    case HeightGain.Slight:
                        yPos += 1.6f + (float)rand.NextDouble() * 0.8f;
                        break;
                    case HeightGain.Medium:
                        yPos += 2.0f + (float)rand.NextDouble() * 1.1f + (float) rand.NextDouble() * 1.1f;
                        break;
                    case HeightGain.Steep:
                        if(jumpBack)    yPos += 2.0f + (float)rand.NextDouble() * 1.9f;
                        else    yPos += 5.1f + (float)rand.NextDouble() * 2.0f;
                        break;
                }
                climbChance = 0.04f + 0.88f * Mathf.Pow((maxHeight - height) / (float)maxHeight, 1.7f);
            }
            else
            {
                switch(heightIncrement)
                {
                    case HeightIncrementType.Linear:
                        switch(heightGain)
                        {
                            case HeightGain.Trivial:
                                climbChance += 0.25f;
                                break;
                            case HeightGain.Slight:
                                climbChance += 0.34f;
                                break;
                            case HeightGain.Medium:
                                climbChance += 0.45f;
                                break;
                            case HeightGain.Steep:
                                climbChance += 0.68f;
                                break;
                        }
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
        tempRope.transform.GetChild(0).GetComponent<Rope>().id = currentID++;

        if(rand.NextDouble() < obsSpawn)
        {
            // Create an obstacle
            double obsChoice = rand.NextDouble();

            if(!jumpBack && obsChoice < 0.25) // Spinny bois
            {
                float obstPos;
                if(climb)   obstPos = -2.5f;
                else    obstPos = 2.85f;
                GameObject tempObstacle = Instantiate(rotatingObstacle, new Vector2((xPos + lastX)/2.0f, 0.35f*yPos+0.65f*lastY + obstPos), Quaternion.identity);
                var tempScript = tempObstacle.GetComponent<RotatingObstacleHinge>(); 
                //tempScript.SetLength(Mathf.Min(averageRopeSpacing * spacingChange/5.8f, Vector2.Distance(tempObstacle.transform.position, tempRope.transform.position)));
                float lengthOffset;
                if(climb)   lengthOffset = -2.5f;
                else        lengthOffset = -1.5f;
                tempScript.SetLength( (Vector2.Distance(tempObstacle.transform.position, new Vector2(lastX, lastY))/2f + lengthOffset) * 4.3f );
                tempScript.SetTorque(7.6f * (-12f + (float)rand.NextDouble()));
                tempScript.SetHingePoint(0f);
            }
            else if(obsChoice < 0.68)  // Blockers
            {
                float dx = (float)(rand.NextDouble()) * 0.2f + 0.45f;   // Between 0.45 and 0.65
                float obstacleX = (dx*xPos + (1f-dx)*lastX);
                float dy = Mathf.Pow((float)(rand.NextDouble() + 0.5) / 2f, 2f) + 0.1875f;   // Between 0.25 and 0.75, weighted towards edges
                float obstacleY;
                if(climb)   obstacleY = lastY - 3.0f + (float)rand.NextDouble() * 2.4f;
                else
                {
                    obstacleY = (dy * yPos + (1f-dy) * lastY) - 8.5f + (Mathf.Pow(dy + 0.5f, 1.5f) - 0.5f) * 8.5f;
                    if(Math.Abs(obstacleX - lastX) < 6.0f)
                    {
                        obstacleY = 0.9f*lastY + 0.1f*yPos - 4.0f + (float)rand.NextDouble() * 1.9f;
                        if(obstacleX > lastX)   obstacleX += 1.2f;
                        else    obstacleX -= 1.2f;
                    }
                }
                
                GameObject tempBlockObstacle = Instantiate(blocker, new Vector2(obstacleX, obstacleY), Quaternion.identity);
                tempBlockObstacle.GetComponent<Blocker>().SetAnimationStart((float)rand.NextDouble() * 1.9f);
            }
            else    // Spikes
            {
                float dx = (float)(rand.NextDouble()) * 0.2f + 0.4f;   // Between 0.4 and 0.6
                float obstacleX = (dx*xPos + (1f-dx)*lastX);
                float obstacleY = 0.85f*lastY + 0.15f*yPos - 6.2f + (float)rand.NextDouble() * 1.9f;

                if(Math.Abs(obstacleX - lastX) < 6.0f)
                {
                    obstacleY = 0.9f*lastY + 0.1f*yPos - 4.0f + (float)rand.NextDouble() * 1.9f;
                    if(obstacleX > lastX)   obstacleX += 1.2f;
                    else    obstacleX -= 1.2f;
                }
                
                GameObject tempSpike = Instantiate(spike, new Vector2(obstacleX, obstacleY), Quaternion.identity);
                // if(dx < 0.48 && rand.NextDouble() < 0.5) Instantiate(spike, new Vector2(obstacleX + 1.3f, obstacleY), Quaternion.identity);
            }

        }

        lastX = xPos;
        lastY = yPos;
        jumpBack = false;
        return new Vector2(xPos, yPos);
        // tempRope.transform.GetChild(0).gameObject.GetComponent<Rope>().SetLength(spacingChange + 1.1f);
        // Debug.Log(height + " " + climbChance);
        // return new Vector2(xPos, 1.5f * ((float)height) + lastY);
    }

    public static int GetMaxRopes()
    {
        return maxRopes;
    }
}
