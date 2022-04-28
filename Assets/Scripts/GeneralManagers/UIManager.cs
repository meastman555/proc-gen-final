using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //treats the UI manager as a singleton even though it doesn't really store much data, just so it can be easily accessed anywhere
    private static UIManager _instance;
    public static UIManager Instance {
        get { return _instance; }
    }

    //carries over player-selected settings from the menu to the game
    //*code/architecture adapted from another singleton I wrote that achieved a similar purpose for another game in another class*
    void Awake() {
        if(_instance != null && _instance != this) {
            Destroy(gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [SerializeField] private GameObject playerHealthBar;
    [SerializeField] private GameObject speedIcon;
    [SerializeField] private GameObject shieldIcon;
    [SerializeField] private GameObject damageIcon;

    //helper functions called when a powerup is activiated, to update UI and show the player when one is active and not
    public void ToggleSpeedIcon(bool toggle) { speedIcon.SetActive(toggle); }
    public void ToggleShieldIcon(bool toggle) { shieldIcon.SetActive(toggle); }
    public void ToggleDamageIcon(bool toggle) { damageIcon.SetActive(toggle); }

    public void SetUIMaxHealth(int maxHealth) { playerHealthBar.GetComponent<Slider>().maxValue = maxHealth; }
    public void UpdatePlayerHealthUI(int health) { playerHealthBar.GetComponent<Slider>().value = health; }
}
