using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//some easy ported over movement code from a previous game jam to test room navigation and collision
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 10.0f;

    private Rigidbody2D rb;
    private Transform childCamera;
    private Quaternion staticCameraRotation;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        childCamera = transform.GetChild(0);
        staticCameraRotation = childCamera.transform.rotation;
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

    //Code adapted from this Unity forum: https://answers.unity.com/questions/731922/rotate-object-to-face-mouse-2d-rotate-on-z-axis.html
    //all the 3d rotation stuff I was trying with Quarternions and LookAt (based on my knowledge of cameras from graphics) was wonky when trying to go back to Unity 2d
    //so I opted to use this solution that is all over the internet (the camera rotation save/restore is my own addition)
    public void RotateToCursor() {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //we don't want the camera to rotate with the player, but it's a child
        //so save it's original rotation, apply player rotation, then restore original camera rotation
        childCamera.transform.rotation = staticCameraRotation;
    }

}
