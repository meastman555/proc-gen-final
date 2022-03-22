using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//some easy ported over movement code from a previous game jam to test room navigation and collision
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 10.0f;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //delegates called from UserInput
    //TODO: add other things like animations?
    public void MoveUp() { 
        rb.velocity = Vector2.up * speed; 
    }

    public void MoveDown() { 
        rb.velocity = Vector2.down * speed; 
    }

    public void MoveLeft() { 
        rb.velocity = Vector2.left * speed; 
    }

    public void MoveRight() { 
        rb.velocity = Vector2.right * speed; 
    }

    public void StopMoving() { 
        rb.velocity = Vector2.zero; 
    }

}
