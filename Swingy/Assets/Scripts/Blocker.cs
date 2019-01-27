using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : Obstacle
{
    bool timerToStartAnimation = false;
    float currTime = 0.0f;
    float goalTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timerToStartAnimation)
        {
            currTime += Time.deltaTime;
            if(currTime > goalTime)
            {
                timerToStartAnimation = false;
                currTime = 0.0f;
                GetComponent<Animator>().enabled = true;
            }
        }
    }

    public void SetAnimationStart(float timeUntilStart)
    {
        goalTime = timeUntilStart;
        currTime = 0.0f;
        timerToStartAnimation = true;
    }
}
