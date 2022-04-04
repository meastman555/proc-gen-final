using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//prefabbing this causes the enemy calculation stuff to not work, so don't prefab! not sure why that happens
public class TimerAndWinLoseState : MonoBehaviour
{

    private static TimerAndWinLoseState _instance;
    public static TimerAndWinLoseState Instance {
        get { return _instance; }
    }

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private int easyTimerSeconds;
    [SerializeField] private int mediumTimerSeconds;
    [SerializeField] private int hardTimerSeconds;

    [SerializeField] private TextMeshProUGUI enemiesRemainingText;

    private int totalTime;
    private int totalEnemies;

    //not technically a needed singleton pattern, but makes sense to treat it like one
    void Awake() {
        if(_instance != null && _instance != this) {
            Destroy(gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeEnemiesText();
        StartTimer();
    }

    //calculates the number of enemies (once) to fill the text with
    private void InitializeEnemiesText() {
        totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemiesRemainingText.text = "Enemies Remaining: " + totalEnemies;
    }

    //gets called whenever an enemy dies, to update the text and check for a win
    public void UpdateEnemiesTextOnKill() {
        totalEnemies--;
        enemiesRemainingText.text = "Enemies Remaining: " + totalEnemies;
        
        //TODO: actual win stuff
        if(totalEnemies == 0) {
            Debug.Log("Game Over! You win!");
        }
    }

    //determines which time to use based on difficulty
    private void StartTimer() {
        InitialSettingsSingleton.Difficulty diff = InitialSettingsSingleton.Instance.GetDifficulty();
        switch(diff) {
            case InitialSettingsSingleton.Difficulty.Easy: {
                totalTime = easyTimerSeconds;
                break;
            }
            case InitialSettingsSingleton.Difficulty.Medium: {
                totalTime = mediumTimerSeconds;
                break;
            }
            case InitialSettingsSingleton.Difficulty.Hard: {
                totalTime = hardTimerSeconds;
                break;
            }
            //if for some reason invalid difficulty, makes medium
            default: {
                totalTime = mediumTimerSeconds;
                break;
            }
        }

        StartCoroutine(TimerTick());
    }

    private IEnumerator TimerTick() {
        while(totalTime > 0) {
            int minutes = totalTime / 60;
            int seconds = totalTime % 60;
            timerText.text = string.Format("Time Remaining: {0:00}:{1:00}", minutes, seconds);
            yield return new WaitForSeconds(1);
            totalTime--;
        }
        
        //TODO: actual lose stuff
        timerText.text = "Time Remaining: 0:00";
        Debug.Log("Game Over! You lose");
    }
}
