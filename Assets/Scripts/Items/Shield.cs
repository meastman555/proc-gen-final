using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{   
    //TODO: duration based for now, maybe hit based is more interesting?
    //incorporate difficulty? Add UI (or shield around player) so the player knows how long the shield lasts!
    [SerializeField] private float duration;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<CombatBehavior>().GiveShield(duration);
            Destroy(this.gameObject);
        }
    }
}
