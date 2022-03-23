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

    //TODO: add other things like animations?
    public void MoveUp() { 
        rb.velocity = Vector2.up * moveSpeed; 
    }

    public void MoveDown() { 
        rb.velocity = Vector2.down * moveSpeed; 
    }

    public void MoveLeft() { 
        rb.velocity = Vector2.left * moveSpeed; 
    }

    public void MoveRight() { 
        rb.velocity = Vector2.right * moveSpeed; 
    }

    public void StopMoving() { 
        rb.velocity = Vector2.zero; 
    }

}
