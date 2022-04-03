using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//carries over player-selected settings from the menu to the game
//generation, etc
//*adapted from another singleton I wrote that achieved a similar purpose for another game in another class*
public class InitialSettingsSingleton : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard };

    //boilerplate for access and setting singleton instance
    private static InitialSettingsSingleton _instance;
    public static InitialSettingsSingleton Instance {
        get { return _instance; }
    }

    void Awake() {
        if(_instance != null && _instance != this) {
            Destroy(gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //the data that needs to be persisted 
    //uses getters and setters which is kinda :/ but there's not much so it's ok
    //TODO: more data (like generation size, depth, etc)! after assignment wednesday is met
    private Difficulty difficulty;

    public Difficulty GetDifficulty() {
        return difficulty;
    }    
    public void SetDifficulty(Difficulty d) {
        difficulty = d;
    }
}