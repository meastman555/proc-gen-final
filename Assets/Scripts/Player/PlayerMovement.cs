using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public functions are called from UserInput.cs
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    //TODO: add other things like animations? sound effects?
    public void MoveVertically(int dir) {
        rb.velocity = Vector2.up * moveSpeed * dir;
    }

    public void MoveHorizontally(int dir) {
        rb.velocity = Vector2.right * moveSpeed * dir;
    }

    public void StopMoving() { 
        rb.velocity = Vector2.zero; 
    }

}
