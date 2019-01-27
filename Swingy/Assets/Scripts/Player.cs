using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    public Rope rope;
    public float launchCoefficient = 3.0f;
    public float mass = 0.8f;
    public float maxYBeforeDeath = 15.0f;
    public ParticleSystem ropeCollision;
    public float audioVariance = 1.5f;
    public GameObject housePrefab;
    public bool pickingColors = false;
    public float axisMultiplier = 1f;

    private Rigidbody2D rb;
    private Camera cam;
    private AudioSource audioSource;
    private Vector2 cameraOffset;
    private bool camPostMove;
    private float lastRopeY; // Used to determine if the player should die
    private bool hasDied;
    private RopeParticleManager particleManager;
    
    private int furthestRope = 0; // High scores!

    // Start is called before the first frame update
    void Start(){
        rope = gameObject.GetComponentInParent<Rope>();
        cam = FindObjectOfType<Camera>();
        audioSource = gameObject.GetComponent<AudioSource>();
        cameraOffset = cam.transform.position - this.transform.parent.gameObject.transform.parent.transform.position;
        cam.SetOffset(cameraOffset);
        particleManager = FindObjectOfType<RopeParticleManager>();
        audioSource.volume = 0;
    }

    // Update is called once per frame
    void Update(){
        if (pickingColors) return;

        if (rope){
            //Rope Movement
            float rawHorizontal = 0;
            rawHorizontal = Input.GetAxisRaw("Horizontal");

            if(rawHorizontal > 0){
                rope.ForceRight(axisMultiplier * rawHorizontal);
            }
            else if(rawHorizontal < 0){
                rope.ForceLeft(axisMultiplier * rawHorizontal);
            }

            //Jumping
            if(Input.GetButtonDown("Jump")){
                launch();
                audioSource.pitch = Random.Range(1 / audioVariance, 1f);
                audioSource.Play();
            }
        }
        else {
            cam.Move(this.transform.position);

            if (lastRopeY - transform.position.y > maxYBeforeDeath)
            {
                initDeath(transform.position, GetComponent<Rigidbody2D>());
            } else
            {
                gameObject.transform.eulerAngles = new Vector3(0, 0, -Mathf.Rad2Deg * Mathf.Atan(rb.velocity.x / rb.velocity.y));
            }
        }

    }

    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Rope")) {
            grabRope(collision.gameObject);
            audioSource.pitch = Random.Range(1f, 1 * audioVariance);
            audioSource.Play();
            particleManager.spawnRopeParticle(collision.GetContact(0).point);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("House")) {
            GetComponent<SpriteRenderer>().enabled = false;
            Vector3 vel = GetComponent<Rigidbody2D>().velocity;
            vel = new Vector3(vel.x * 0.001f, vel.y * 0.001f, vel.z * 0.001f);
            // Set house colors
            // Fire off some particle effects if applicable, prompt "R" to restart
            collider.gameObject.GetComponent<House>().init(furthestRope);
        }
    }

    void launch(){
        StopAllCoroutines();
        if(audioSource.volume == 0){
            audioSource.volume = 1;
        }
        rope.destroyCollider();
        gameObject.AddComponent<BoxCollider2D>();
        lastRopeY = rope.transform.parent.position.y;

        gameObject.AddComponent<Rigidbody2D>();
        rb = gameObject.GetComponent<Rigidbody2D>(); 
        rb.velocity = launchCoefficient * rope.GetVelocity();
        rb.mass = mass;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

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
            transform.localPosition = Vector3.Lerp(startPosition, new Vector3(0f, -1.34f, 0f), elapsedTime / time);
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
        
        lastRopeY = 0.0f;
        Rope r = grabbedRope.GetComponent<Rope>();
        furthestRope = r.id > furthestRope ? r.id : furthestRope;
    }


    private void initDeath(Vector3 position, Rigidbody2D playerRB)
    {
        if (hasDied) return;
        float time = 1.0f;

        float newX = position.x + playerRB.velocity.x * time;
        float newY = position.y + playerRB.velocity.y * time - (1f/2f) * (9.8f * playerRB.gravityScale) * Mathf.Pow(time,2);

        Instantiate(housePrefab, new Vector3(newX, newY, -1.17f), Quaternion.identity);
        hasDied = true;
        rb.freezeRotation = true;
    }
}
