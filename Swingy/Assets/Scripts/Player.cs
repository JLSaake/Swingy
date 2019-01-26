using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    public Rope rope;
    public float launchCoefficient = 2;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start(){
        rope = gameObject.GetComponentInParent<Rope>();
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
                launch();
            }
        }
        else{
            gameObject.transform.eulerAngles = new Vector3(0, 0, -Mathf.Rad2Deg * Mathf.Atan(rb.velocity.x / rb.velocity.y));
        }
    }

    void launch(){
        gameObject.AddComponent<Rigidbody2D>();
        rb = gameObject.GetComponent<Rigidbody2D>(); 
        rb.velocity = launchCoefficient * rope.GetVelocity();
        transform.parent = null;
        rope = null;
    }
}
