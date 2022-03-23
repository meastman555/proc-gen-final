using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //bullet gets destroyed regardless of what it collides with
    //more complex logic needed on collision will be handled in respective game objects using delegates
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("colliding! with: " + other.gameObject.tag);
    
        if(other.gameObject.tag == "Enemy") {
            //TODO: call enemy callback to handle logic before destroying
            Debug.Log("bullet hit enemy!");
        }
        Destroy(this.gameObject);
    }
}
