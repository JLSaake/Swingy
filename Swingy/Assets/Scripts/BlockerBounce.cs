using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockerBounce : MonoBehaviour
{
    GameObject player;
    Rigidbody2D playerRB;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        // Debug.Log(player);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log(coll.collider.gameObject.name);
        if(coll.collider.gameObject == player)
        {
            playerRB = player.GetComponent<Rigidbody2D>();
            
            playerRB.velocity = new Vector2((playerRB.velocity.x + 9.2f) * 1.64f,
                playerRB.velocity.y + 8.5f);
        }
    }
}
