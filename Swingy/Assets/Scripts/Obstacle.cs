using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Obstacle : MonoBehaviour
{
    private System.Random rand;

    // Start is called before the first frame update
    void Start()
    {
        // Some commented out line of code which is supposed to be useful
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRandom(System.Random rand)
    {
        this.rand = rand;
    }
}
