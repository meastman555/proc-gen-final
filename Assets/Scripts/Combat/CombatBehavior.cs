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
    private bool doubleDamageActive;
    private bool shieldActive;

    void Awake() {
        health = baseHealth + Random.Range(0, maxExtraHealth + 1);
        doubleDamageActive = false;
        shieldActive = false;
    }

    public int CalculateDamage() {
        int damage = baseDamage + Random.Range(0, maxExtraDamage + 1);
        //accounts for powerup
        damage = doubleDamageActive ? (damage * 2) : damage;
        return damage;
    }

    //entity takes damage, account for death if it happens
    public void ReceiveDamage(int damageDealt) {
        //account for player shield (will always be false for enemy damage since they can never get shield)
        if(shieldActive) {
            Debug.Log("Shield prevented damage!");
            return;
        }

        //no else, because the early return acts as a guard clause
        //general damage taking case for both player and enemy
        Debug.Log(gameObject.name + " got hit for " + damageDealt + " damage!");
        health -= damageDealt;
        //cap at 0
        health = Mathf.Clamp(health, 0, 100);
        //TODO: UI healthbar stuff

        //death! do stuff depending on if this is enemy or player
        if(health <= 0) {
            Destroy(gameObject);
            if(gameObject.tag == "Enemy") {
                TimerAndWinLoseState.Instance.UpdateEnemiesTextOnKill();
            }
            else {
                Debug.Log("Game Over! You died.");
            }
        }
    }

    //called from health pack powerup, only on player -- enemies have no way of restoring health
    public void RestoreHealth(int healthHealed) {
        Debug.Log("Healed for: " + healthHealed + " points!");
        health += healthHealed;
        //cap at 100
        health = Mathf.Clamp(health, 0, 100);
        Debug.Log("Health is now: " + health);
        //TODO: UI healthbar stuff
    }

    //called from double damage powerup, only on player -- enemies have no way of getting double damage
    //multiple double damages do not stack
    public void GiveDoubleDamage(float duration) {
        if(!doubleDamageActive) {
            Debug.Log("Player double damage for: " + duration + " seconds!");
            StartCoroutine(HandleDoubleDamage(duration));
        }
    }

    private IEnumerator HandleDoubleDamage(float duration) {
        doubleDamageActive = true;
        yield return new WaitForSeconds(duration);
        doubleDamageActive = false;
    }

    //called from shield powerup, only on player -- enemies have no way of acquiring shield
    //multiple shields do not stack
    public void GiveShield(float duration) {
        if(!shieldActive) {
            Debug.Log("Player shield active for: " + duration + " seconds!");
            StartCoroutine(HandleShield(duration));
        }
    }

    private IEnumerator HandleShield(float duration) {
        shieldActive = true;
        yield return new WaitForSeconds(duration);
        shieldActive = false;
    }
}
