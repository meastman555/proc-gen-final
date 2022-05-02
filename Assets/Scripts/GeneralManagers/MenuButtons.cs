using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject mainSubMenu;
    [SerializeField] private GameObject optionsSubMenu;
    [SerializeField] private GameObject howToPlaySubMenu;
    [SerializeField] private GameObject creditsSubMenu;
    [SerializeField] private GameObject backButton;
    [SerializeField] private TextMeshProUGUI difficultyText;

    void Start() {
        ReturnToMainMenu();
    }

    //returns to main menu by disabling all other menus
    public void ReturnToMainMenu() {
        mainSubMenu.SetActive(true);
        optionsSubMenu.SetActive(false);
        creditsSubMenu.SetActive(false);
        howToPlaySubMenu.SetActive(false);
        backButton.SetActive(false);
    }

    //TODO: put on a delay/some kind of transition?
    public void StartGame(string gameSceneName) {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame() {
        Application.Quit();
    }

    //tried an enum instead of string, but couldn't get it to serialize so I couldn't include it as passed parameter on menu button click callback (this function didn't pop up)
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

    public void LoadMainMenuScene(string menuSceneName) {
        SceneManager.LoadScene(menuSceneName);
    }
}
