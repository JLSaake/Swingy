using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    public Rope rope;

    // Start is called before the first frame update
    void Start(){
        rope = gameObject.GetComponentInParent<Rope>();
    }

    void launch(){
        transform.parent = null;
        rope = null;
    }

    // Update is called once per frame
    void Update(){
        if(rope){
            //Rope Movement
            float rawHorizontal = 0;
            rawHorizontal = Input.GetAxisRaw("Horizontal");
            // Debug.Log(rawHorizontal);

            if(rawHorizontal > 0){
                rope.ForceRight(rawHorizontal);
            }
            else if(rawHorizontal < 0){
                rope.ForceLeft(rawHorizontal);
            }

            if(Input.GetButtonDown("Jump")){
            }
        }
    }
}
