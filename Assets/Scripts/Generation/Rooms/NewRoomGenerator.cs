using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRoomGenerator : MonoBehaviour
{
    [SerializeField] private Transform roomContainer;
    //X max
    [SerializeField] private int width;
    //Y max
    [SerializeField] private int height;
    
    private int adjustedWidth;
    private int adjustedHeight;

    private WangTiles wang;
    //internal storage of rooms, adds in a buffer so need to adjust some variables
    private GameObject[,] rooms;


    void Start() {
        wang = GetComponent<WangTiles>();

        adjustedWidth = width + 2;
        adjustedHeight = height + 2;

        ResetAll();
    }

    public void ResetAll() {
        rooms = new GameObject[adjustedWidth, adjustedHeight];
        foreach(Transform child in roomContainer.transform) {
            Destroy(child.gameObject);
        }
        GenerateAll();
    }

    private void GenerateAll() {
        GenerateBaseLayout();
        PassThroughGrammar();
        SpawnEnemies();
        SpawnObjects();
        //TODO: add any other needed generation layers!
    }

    //TODO: implement! generates just the blank tiles for structure
    //rooms is passed by reference to wang tiles, and it's where the final layout ends up internally
    //in the game/editor, rooms are in the container parent object
    private void GenerateBaseLayout() {
        wang.GenerateRooms(roomContainer, rooms);
    }

    //TODO: implement! passes all the blank tiles through the room type grammar
    //this may be able to be implementd in GenerateBaseLayout if it proves to not be necessary or minimal work
    private void PassThroughGrammar() {

    }

    //TODO: implement! handles spawning logic for enemies in the enemy type rooms
    private void SpawnEnemies() {

    }

    //TODO: implement! handles spawning logic for objects (power-ups, etc...) in item/object type rooms
    private void SpawnObjects() {

    }

}
