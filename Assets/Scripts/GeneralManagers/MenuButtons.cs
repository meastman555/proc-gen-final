using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private string gameSceneName;

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
}
