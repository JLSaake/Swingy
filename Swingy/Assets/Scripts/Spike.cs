using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Obstacle
{

    public float maxScale = 2;
    public float upSpeed = 0.2f; // Time taken for spike to go up
    public float downSpeed; // Time taken for spike to go back down
    private Vector3 downLocation;
    private Vector3 upLocation;

    // Start is called before the first frame update
    void Start()
    {
        downSpeed = upSpeed * 3;
        Calculate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Calculate()
    {
        downLocation = this.transform.position;
        if (this.transform.eulerAngles.z > 90 || this.transform.eulerAngles.z < -90)
        {
            upLocation = new Vector3(this.transform.position.x, 
                                     this.transform.position.y - (maxScale - this.transform.localScale.y / 2),
                                     this.transform.position.z);
        } else
        {
            upLocation = new Vector3(this.transform.position.x,
                         this.transform.position.y - (maxScale + this.transform.localScale.y / 2),
                         this.transform.position.z);
        }
    }

    
    IEnumerator SpikeUp()
    {
        Vector2 startScale = gameObject.transform.localScale;
        Vector2 endScale = new Vector3(gameObject.transform.localScale.x, maxScale, gameObject.transform.localScale.z);

        float elapsedTime = 0f;
        float time = 0.1f;

        while (elapsedTime < time)
        {
            transform.localPosition = Vector3.Lerp(downLocation, upLocation, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
}
