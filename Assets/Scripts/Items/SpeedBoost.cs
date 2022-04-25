using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    //TODO: incorporate difficulty? Add UI (or effect around player) so player knows how long to boost lasts!
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float duration;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<PlayerMovement>().GiveSpeedBoost(speedMultiplier, duration);
            Destroy(this.gameObject);
        }
    }
}
