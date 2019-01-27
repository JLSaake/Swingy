using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    public Rope rope;
    public float launchCoefficient = 3;
    public float mass = 0.8f;
    public ParticleSystem ropeCollision;
    public float audioVariance = 1.5f;

    private Rigidbody2D rb;
    private Camera cam;
    private AudioSource audioSource;
    private Vector2 cameraOffset;
    private bool camPostMove;

    // Start is called before the first frame update
    void Start(){
        rope = gameObject.GetComponentInParent<Rope>();
        cam = FindObjectOfType<Camera>();
        audioSource = gameObject.GetComponent<AudioSource>();
        cameraOffset = cam.transform.position - this.transform.parent.gameObject.transform.parent.transform.position;
        cam.SetOffset(cameraOffset);
    }

    // Update is called once per frame
    void Update(){

        if (rope){
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
                audioSource.pitch = Random.Range(1 / audioVariance, 1f);
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else{
            gameObject.transform.eulerAngles = new Vector3(0, 0, -Mathf.Rad2Deg * Mathf.Atan(rb.velocity.x / rb.velocity.y));
            cam.Move(this.transform.position);
        }

    }

    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Rope")) {
            grabRope(collision.gameObject);
            audioSource.pitch = Random.Range(1f, 1 * audioVariance);
            gameObject.GetComponent<AudioSource>().Play();
            SpawnParticles(collision.GetContact(0).point);
        }
    }

    void launch(){
        StopAllCoroutines();
        rope.destroyCollider();
        gameObject.AddComponent<BoxCollider2D>();

        gameObject.AddComponent<Rigidbody2D>();
        rb = gameObject.GetComponent<Rigidbody2D>(); 
        rb.velocity = launchCoefficient * rope.GetVelocity();
        rb.mass = mass;

        cam.SetOffset(cam.transform.position - this.transform.position);

        transform.parent = null;
        rope = null;
    }

    // IEnumerator center(){
    //     Vector3 startPosition = gameObject.transform.localPosition;
    //     Vector3 startRotation = gameObject.transform.localEulerAngles;
    //     // Vector3 destination = new Vector3(0, gameObject.transform.localPosition.y, 0);
    //     // Vector3 destination = new Vector3(0, -2, 0);
    //     float elapsedTime = 0f;
    //     float time = 0.5f;

    //     while(elapsedTime < time){
    //         transform.localPosition = Vector3.Lerp(startPosition, new Vector3(0, -2, 0), elapsedTime / time);
    //         transform.localEulerAngles = Vector3.Lerp(startRotation, new Vector3(0, 0, 0), elapsedTime / time);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }
    // }

    // IEnumerator center(){
    //     Vector3 startPosition = gameObject.transform.localPosition;
    //     float elapsedTime = 0f;
    //     float time = 0.5f;

    //     while(elapsedTime < time){
    //         transform.localPosition = Vector3.Lerp(startPosition, new Vector3(0, -2, 0), elapsedTime / time);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }
    // }

    IEnumerator center(){
        Vector3 startPosition = gameObject.transform.localPosition;
        float elapsedTime = 0f;
        float time = 0.5f;

        while(elapsedTime < time){
            transform.localPosition = Vector3.Lerp(startPosition, new Vector3(0f, -2f, 0f), elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator spin(){
        Vector3 startRotation = gameObject.transform.localEulerAngles;
        float elapsedTime = 0f;
        float time = 1f;

        while(elapsedTime < time){
            transform.localEulerAngles = Vector3.Lerp(startRotation, new Vector3(0f, 0f, 0f), elapsedTime / time);
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

        StartCoroutine(center());
        StartCoroutine(spin());

        cam.SetOffset(cameraOffset);
        StartCoroutine(cam.MoveToRope(this.transform.parent.gameObject.transform.parent.transform.position));

    }

    private void SpawnParticles(Vector2 pos)
    {
        ParticleSystem particle = Instantiate(ropeCollision, pos, Quaternion.identity);
        particle.Play();
        Destroy(particle.gameObject, particle.main.duration + 1);
    }
}
