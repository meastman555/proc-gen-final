using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    [SerializeField] private TextMeshProUGUI difficultyText;

    //TODO: add stuff for difficulty (easy, medium, hard, ???)
    //should call another game object function (probably a singleton) that stores this difficulty
    //each thing in the level now needs parameters for each difficulty (player damage, enemy spawn rate/health, item effectiveness, etc)
    //specify all of those in editor, then on generation pull them from whichever difficulty singleton stores
    //this singleton can be used for other user starting options, like player class, random seed, etc.

    //TODO: look into how to do like an options submenu? so that hitting options would go to another UI screen with options
    //or put them on main generation screen so user knows that they're worth looking in to

    //TODO: put on a delay/some kind of transition?
    public void StartGame() {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame() {
        Application.Quit();
    }

    //tried an enum but couldn't get it to serialize so I couldn't include it as passed parameter on menu button click callback (this function didn't pop up)
    public void SetDifficulty(string d) {
        difficultyText.text = "Select Difficulty: " + d;

        //default difficulty is medium
        InitialSettingsSingleton.Difficulty diff = InitialSettingsSingleton.Difficulty.Medium;
        if(d == "Easy") {
            diff = InitialSettingsSingleton.Difficulty.Easy;
        }
        else if(d == "Hard") {
            diff = InitialSettingsSingleton.Difficulty.Hard;
        }
        InitialSettingsSingleton.Instance.SetDifficulty(diff);
    }
}
