using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveTime;
    [SerializeField] private float moveDelay;

    private Rigidbody2D rb;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(RandomMovement());
    }

    //picks a random direction and moves in it
    //speed, time of walk, and time between walk all specified in editor
    private IEnumerator RandomMovement() {
        //this is fine bc it's a coroutine and when the enemy is killed (object destroyed) this will of course stop running
        while(true) {
            float randHorizontalDir = Random.Range(-1.0f, 1.0f);
            float randVerticalDir = Random.Range(-1.0f, 1.0f);

            Vector2 moveDir = (new Vector2(randHorizontalDir, randVerticalDir).normalized) * moveSpeed;
            rb.velocity = moveDir;
            yield return new WaitForSeconds(moveTime);
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(moveDelay);
        }
    }

    //to help not get stuck on walls
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Room") {
            rb.velocity = -other.relativeVelocity;
        }    
    }
}
