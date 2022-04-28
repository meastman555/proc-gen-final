using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public functions are called from UserInput.cs
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float invulnerabilityPeriod;

    private Rigidbody2D rb;
    private bool speedBoostActive;
    private bool canTakeDamage;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        speedBoostActive = false;
        canTakeDamage = true;
    }

    //TODO: add other things like animations? sound effects?
    public void MoveVertically(int dir) {
        rb.velocity = Vector2.up * moveSpeed * dir;
    }

    public void MoveHorizontally(int dir) {
        rb.velocity = Vector2.right * moveSpeed * dir;
    }

    public void StopMoving() { 
        rb.velocity = Vector2.zero; 
    }

    //multiple speed boosts do not stack
    public void GiveSpeedBoost(float speedMultiplier, float duration) {
        if(!speedBoostActive) {
            Debug.Log("Giving player " + speedMultiplier + " speed boost multiplier for " + duration + " seconds!");
            StartCoroutine(HandleSpeedBoost(speedMultiplier, duration));          
        }
    }

    private IEnumerator HandleSpeedBoost(float speedMultiplier, float duration) {
        speedBoostActive = true;
        moveSpeed *= speedMultiplier;
        UIManager.Instance.ToggleSpeedIcon(true);
        yield return new WaitForSeconds(duration);
        moveSpeed /= speedMultiplier;
        speedBoostActive = false;
        UIManager.Instance.ToggleSpeedIcon(false);
    }

    //enemies attack by touching player
    //but player cannot be spam attacked -- there is a window of invulnerability after receiving damage (and to keep the collisions from racking up really fast)
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Enemy" && canTakeDamage) {
            int damage = other.gameObject.GetComponent<EnemyCombat>().CalculateDamage();
            GetComponent<PlayerCombat>().ReceiveDamage(damage);
            StartCoroutine(PostDamageInvulnerability());
        }
    }

    private IEnumerator PostDamageInvulnerability() {
        canTakeDamage = false;
        yield return new WaitForSeconds(invulnerabilityPeriod);
        canTakeDamage = true;
    }
}
