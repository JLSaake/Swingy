using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Obstacle
{
    public float waveHeight = 1.25f;
    public float waveSpeed = 0.05f;
    private Vector2 startLocation;
    private float negCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        startLocation = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        negCounter -= waveSpeed;

        this.gameObject.transform.position = new Vector2(startLocation.x + negCounter, startLocation.y + Mathf.Sin(negCounter) * waveHeight);
    }


}
