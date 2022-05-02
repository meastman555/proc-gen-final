using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//tried to do some more complex things with menus/score persistence/merging the win and lose screens to just show the correct text
//but I'll be honest this was the night before it was due so I eventually ended up going the naive, kinda hacky route just to get it in
public class EndgameScreens : MonoBehaviour
{
    [SerializeField] private string winSceneName;

    //hacky way to determine and handle the win
    void Start() {
        if(SceneManager.GetActiveScene().name == winSceneName) {
            TextMeshProUGUI scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
            int score = PlayerPrefs.GetInt("score");
            scoreText.text = "Score (to hold on to for bragging rights): " + score;
        }
    }

    public void ReturnToMainMenu(string menuSceneName) {
        //some singletons could persist over and need to be destroyed
        //so they can be correctly reset for the next run
        Destroy(TimerAndWinLoseState.Instance.gameObject);
        Destroy(UIManager.Instance.gameObject);
        Destroy(InitialSettingsSingleton.Instance.gameObject);
        SceneManager.LoadScene(menuSceneName);
    }
}
