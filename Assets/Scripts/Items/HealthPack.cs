using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    //TODO: incorporate difficulty? Add UI (or effect around player) so the player knows the powerup was activated
    [SerializeField] private int baseHealthRegen;
    [SerializeField] private int maxBonusHealthRegen;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player") {
            int restoredHealth = baseHealthRegen + Random.Range(0, maxBonusHealthRegen + 1);
            other.gameObject.GetComponent<CombatBehavior>().RestoreHealth(restoredHealth);
            Destroy(this.gameObject);
        }
    }
}
