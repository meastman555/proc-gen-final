using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private int baseHealth;
    [SerializeField] private int maxExtraHealth;
    [SerializeField] private int baseDamage;
    [SerializeField] private int maxExtraDamage;

    private int health;
    private bool doubleDamageActive;
    private bool shieldActive;

    // Start is called before the first frame update
    void Start() {
        health = baseHealth + Random.Range(0, maxExtraHealth + 1);
        UIManager.Instance.SetUIMaxHealth(health);
        UIManager.Instance.UpdatePlayerHealthUI(health);
        doubleDamageActive = false;
        shieldActive = false;
    }

    public int CalculateDamage() {
        int damage = baseDamage + Random.Range(0, maxExtraDamage + 1);
        //accounts for powerup
        damage = doubleDamageActive ? (damage * 2) : damage;
        return damage;
    }

    public void ReceiveDamage(int damageDealt) {
        //account for player shield
        if(shieldActive) {
            return;
        }

        //no else, because the early return acts as a guard clause
        //general damage taking case for both player and enemy
        health -= damageDealt;
        //cap at 0
        health = Mathf.Clamp(health, 0, 100);
        UIManager.Instance.UpdatePlayerHealthUI(health);
        //death! do stuff depending on if this is enemy or player
        if(health <= 0) {
            Destroy(gameObject);
            TimerAndWinLoseState.Instance.Lose();
        }
    }

     //called from health pack powerup
    public void RestoreHealth(int healthHealed) {
        health += healthHealed;
        //cap at 100
        health = Mathf.Clamp(health, 0, 100);
        //TODO: UI healthbar stuff
        UIManager.Instance.UpdatePlayerHealthUI(health);
    }

    //called from double damage powerup
    //multiple double damages do not stack
    public void GiveDoubleDamage(float duration) {
        if(!doubleDamageActive) {
            StartCoroutine(HandleDoubleDamage(duration));
        }
    }

    private IEnumerator HandleDoubleDamage(float duration) {
        doubleDamageActive = true;
        UIManager.Instance.ToggleDamageIcon(true);
        yield return new WaitForSeconds(duration);
        doubleDamageActive = false;
        UIManager.Instance.ToggleDamageIcon(false);
    }

    //called from shield powerup
    //multiple shields do not stack
    public void GiveShield(float duration) {
        if(!shieldActive) {
            StartCoroutine(HandleShield(duration));
        }
    }

    private IEnumerator HandleShield(float duration) {
        shieldActive = true;
        UIManager.Instance.ToggleShieldIcon(true);
        yield return new WaitForSeconds(duration);
        shieldActive = false;
        UIManager.Instance.ToggleShieldIcon(false);
    }
}
