using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBehavior : MonoBehaviour
{
    [SerializeField] private int baseHealth;
    [SerializeField] private int maxExtraHealth;
    [SerializeField] private int baseDamage;
    [SerializeField] private int maxExtraDamage;

    private int health;
    void Awake() {
        health = baseHealth + Random.Range(0, maxExtraHealth);
    }

    public int CalculateDamage() {
        int damage = baseDamage + Random.Range(0, maxExtraDamage);
        return damage;
    }

    public void ReceiveDamage(int damageDealt) {
        Debug.Log(gameObject.name + " got hit for " + damageDealt + " damage!");
        health -= damageDealt;
        //TODO: UI healthbar stuff

        //enemy death!
        if(health <= 0) {
            Destroy(gameObject);
        }
    }
}
