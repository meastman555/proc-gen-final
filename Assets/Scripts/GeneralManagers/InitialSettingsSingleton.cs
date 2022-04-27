using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InitialSettingsSingleton : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard };

    //boilerplate for access and setting singleton instance
    private static InitialSettingsSingleton _instance;
    public static InitialSettingsSingleton Instance {
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

    //the data that needs to be persisted 
    //uses getters and setters which is kinda :/ but there's not much so it's ok
    //TODO: more data (like generation size, depth, etc)!
    private Difficulty difficulty = Difficulty.Medium;
    private int width = 50;
    private int height = 50;
    private int maxRecusrionDepth = 8;
    private int minRooms = 30;

    //various callbacks to set the user-specified parameters before game start
    public void SetDifficulty(Difficulty d) { difficulty = d; }
    public Difficulty GetDifficulty() { return difficulty; }  
    public void SetWidth(TMP_InputField input) { width = Int32.Parse(input.text); }
    public int GetWidth() { return width; }  

    public void SetHeight(TMP_InputField input) { height = Int32.Parse(input.text); }
    public int GetHeight() { return height; }  

    public void SetMaxRecursionDepth(TMP_InputField input) { maxRecusrionDepth = Int32.Parse(input.text); }
    public int GetMaxRecursionDepth() { return maxRecusrionDepth; }

    public void SetMinRooms(TMP_InputField input) { minRooms = Int32.Parse(input.text); }
    public int GetMinRooms() { return minRooms; } 
}
