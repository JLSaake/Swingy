using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    public Rope rope;
    public float launchCoefficient = 3;
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

            if(rawHorizontal > 0){
                rope.ForceRight(rawHorizontal);
            }
            else if(rawHorizontal < 0){
                rope.ForceLeft(rawHorizontal);
            }

            //Jumping
            if(Input.GetButtonDown("Jump")){
                launch();
            }
        }
        else{
            gameObject.transform.eulerAngles = new Vector3(0, 0, -Mathf.Rad2Deg * Mathf.Atan(rb.velocity.x / rb.velocity.y));
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Rope")){
            grabRope(collision.gameObject);
        }
    }

    void launch(){
        rope.destroyCollider();
        gameObject.AddComponent<BoxCollider2D>();
        gameObject.AddComponent<Rigidbody2D>();
        rb = gameObject.GetComponent<Rigidbody2D>(); 
        rb.velocity = launchCoefficient * rope.GetVelocity();
        transform.parent = null;
        rope = null;
    }

    IEnumerator centerX(){
        Vector3 start = gameObject.transform.localPosition;
        // Vector3 destination = new Vector3(0, gameObject.transform.localPosition.y, 0);
        Vector3 destination = new Vector3(0, -2, 0);
        float elapsedTime = 0f;
        float time = 0.5f;

        while(elapsedTime < time){
            transform.localPosition = Vector3.Lerp(start, destination, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void grabRope(GameObject grabbedRope){
        transform.parent = grabbedRope.transform;
        rope = gameObject.GetComponentInParent<Rope>();

        Destroy(gameObject.GetComponent<BoxCollider2D>());
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        rb = null;

        StartCoroutine(centerX());
    }
}
