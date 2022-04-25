using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDamage : MonoBehaviour
{
    //TODO: incorporate difficulty? Add UI (or effect to gun) so the player knows how long the boost lasts!
    [SerializeField] private float duration;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<CombatBehavior>().GiveDoubleDamage(duration);
            Destroy(this.gameObject);
        }
    }
}
