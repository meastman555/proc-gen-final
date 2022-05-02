using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private int easySecondsPerEnemy;
    [SerializeField] private int mediumSecondsPerEnemy;
    [SerializeField] private int hardSecondsPerEnemy;

    [Header("Scoring")]
    [SerializeField] private int scorePerEnemy;
    [SerializeField] private float easyScoreMultiplier;
    [SerializeField] private float mediumScoreMultiplier;
    [SerializeField] private float hardScoreMultiplier;

    [SerializeField] private TextMeshProUGUI enemiesRemainingText;
    [SerializeField] private GameObject roomContainer;
    [SerializeField] private string winSceneName;
    [SerializeField] private string loseSceneName;

    private int totalTime;
    private int totalEnemies;
    private int score;

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
        score = 0;
        PlayerPrefs.DeleteKey("score");
        InitializeEnemiesText();
        StartTimer();
    }

    //calculates the number of enemies (once) to fill the text with
    //TODO: can speed up just a bit by using the numEnemies data in RoomData (so only have to check each room and not every gameobject by tag)
    private void InitializeEnemiesText() {
        totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemiesRemainingText.text = "Enemies Remaining: " + totalEnemies;
    }

    //gets called whenever an enemy dies, to update the text and check for a win
    public void UpdateEnemiesTextOnKill() {
        totalEnemies--;
        enemiesRemainingText.text = "Enemies Remaining: " + totalEnemies;
        score += scorePerEnemy;
        
        if(totalEnemies == 0) {
            CalculateFinalScore();
            PlayerPrefs.SetInt("score", score);
            SceneManager.LoadScene(winSceneName);
        }
    }

    //determines which time to use based on difficulty
    private void StartTimer() {
        InitialSettingsSingleton.Difficulty diff = InitialSettingsSingleton.Instance.GetDifficulty();
        switch(diff) {
            case InitialSettingsSingleton.Difficulty.Easy: {
                totalTime = totalEnemies * easySecondsPerEnemy;
                break;
            }
            case InitialSettingsSingleton.Difficulty.Medium: {
                totalTime = totalEnemies * mediumSecondsPerEnemy;
                break;
            }
            case InitialSettingsSingleton.Difficulty.Hard: {
                totalTime = totalEnemies * hardSecondsPerEnemy;
                break;
            }
            //if for some reason invalid difficulty, makes medium
            default: {
                totalTime = totalEnemies * mediumSecondsPerEnemy;
                break;
            }
        }
        StartCoroutine(TimerTick());
    }

    private IEnumerator TimerTick() {
        yield return new WaitForSeconds(1.0f);
        while(totalTime > 0) {
            int minutes = totalTime / 60;
            int seconds = totalTime % 60;
            timerText.text = string.Format("Time Remaining: {0:00}:{1:00}", minutes, seconds);
            yield return new WaitForSeconds(1);
            totalTime--;
        }
        timerText.text = "Time Remaining: 0:00";
        Lose();
    }

    //score is multiplied based on difficulty
    private void CalculateFinalScore() {
        score += totalTime;
        InitialSettingsSingleton.Difficulty diff = InitialSettingsSingleton.Instance.GetDifficulty();
        switch(diff) {
            case InitialSettingsSingleton.Difficulty.Easy: {
                score = Mathf.RoundToInt(score * easyScoreMultiplier);
                break;
            }
            case InitialSettingsSingleton.Difficulty.Medium: {
                score = Mathf.RoundToInt(score * mediumScoreMultiplier);
                break;
            }
            case InitialSettingsSingleton.Difficulty.Hard: {
                score = Mathf.RoundToInt(score * hardScoreMultiplier);
                break;
            }
            //if for some reason invalid difficulty, makes medium
            default: {
                score = Mathf.RoundToInt(score * mediumScoreMultiplier);
                break;
            }
        }
    }

    //hacky, but needed (for when player dies)
    public void Lose() {
        SceneManager.LoadScene(loseSceneName);
    }
}
