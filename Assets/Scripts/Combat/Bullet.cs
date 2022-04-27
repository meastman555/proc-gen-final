using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;

    public void SetDamage(int d) {
        damage = d;
    }

    //bullet gets destroyed regardless of what it collides with
    //more complex logic needed on collision will be handled in respective game objects using delegates
    private void OnCollisionEnter2D(Collision2D other) {    
        if(other.gameObject.tag == "Enemy") {
            //send to enemy to handle effects of raw bullet damage
            other.gameObject.GetComponent<CombatBehavior>().ReceiveDamage(damage);
        }
        Destroy(this.gameObject);
    }
}
