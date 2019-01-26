using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentRope : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScale(float y)
    {
        this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x, y, 
                                                         this.gameObject.transform.localScale.z);
    }
}
