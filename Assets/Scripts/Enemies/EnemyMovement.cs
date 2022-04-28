using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //[SerializeField] private GameObject player;
    [SerializeField] private float viewDistance;
    [SerializeField] private float moveSpeed;

    private GameObject player;

    //once the player gets close enough to enemy, it won't stop chasing the player even if they gain distance
    private bool chasingPlayer;

    void Awake() {
        //this is not serialized because enemies are created at runtime, and need the runtime instance of the player (not the prefab)
        player = GameObject.FindGameObjectWithTag("Player");
        chasingPlayer = false;
    }

    void Update() {
        if(!chasingPlayer) {
            CheckForPlayerRange();
        }
        else {
            MoveTowardPlayer();
        }
    }

    //sets the boolean if the player is in range (so enemy should start moving)
    private void CheckForPlayerRange() {
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        chasingPlayer = (distToPlayer <= viewDistance);
    }

    //TODO: since this is tied to transform.position, enemies completely ignore collisions and can phase through walls
    private void MoveTowardPlayer() {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }
}
