using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private int baseHealth;
    [SerializeField] private int maxExtraHealth;
    [SerializeField] private int baseDamage;
    [SerializeField] private int maxExtraDamage;

    private int health;

    // Start is called before the first frame update
    void Start() {
        health = baseHealth + Random.Range(0, maxExtraHealth + 1);
    }

    public int CalculateDamage() {
        int damage = baseDamage + Random.Range(0, maxExtraDamage + 1);
        return damage;
    }

    public void ReceiveDamage(int damageDealt) {
        health -= damageDealt;
        //cap at 0
        health = Mathf.Clamp(health, 0, 100);
        //TODO: enemy healthbar UI?

        //death! do stuff depending on if this is enemy or player
        if(health <= 0) {
            Destroy(gameObject);
            TimerAndWinLoseState.Instance.UpdateEnemiesTextOnKill();
        }
    }
}
