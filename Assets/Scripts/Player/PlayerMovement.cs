using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public functions are called from UserInput.cs
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;
    private bool speedBoostActive;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        speedBoostActive = false;
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
        yield return new WaitForSeconds(duration);
        moveSpeed /= speedMultiplier;
        speedBoostActive = false;
    }
}
