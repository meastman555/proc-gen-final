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
    //middle of grid is not (0,0), it's (width/2, height/2)
    [SerializeField] private int startX;
    [SerializeField] private int startY;

    private int adjustedWidth;
    private int adjustedHeight;
    private int adjustedStartX;
    private int adjustedStartY;

    private WangTiles wang;
    private NewRoomGrammar grammar;

    //internal storage of rooms, adds in a buffer so need to adjust some variables
    //TODO: move this to just be within wang? So far it's not used anywhere else
    //however, if say the grammar wanted to know what surrounding room types are before deciding the next room's (to prevent them from being the same, etc.) then it would be used, so wait on this change
    private GameObject[,] rooms;

    void Start() {
        wang = GetComponent<WangTiles>();
        grammar = GetComponent<NewRoomGrammar>();

        adjustedWidth = width + 2;
        adjustedHeight = height + 2;
        adjustedStartX = startX + 1;
        adjustedStartY = startY + 1;

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
        PassRoomsThroughGrammar();
        GenerateEnemies();
        GenerateObjects();
        //TODO: add any other needed generation layers!
    }

    //generates just the blank tiles for structure
    //rooms is passed by reference to wang tiles, and it's where the final layout ends up internally
    //in the game/editor, rooms are in the container parent object
    private void GenerateBaseLayout() {
        wang.GenerateRooms(roomContainer, rooms, adjustedStartX, adjustedStartY);
    }

    //TODO: implement! passes all the blank tiles through the room type grammar
    //this may be able to be implementd in GenerateBaseLayout if it proves to not be necessary or minimal work
    private void PassRoomsThroughGrammar() {
        grammar.AssignRoomTypes(roomContainer);
    }

    //TODO: implement! handles spawning logic for enemies in the enemy type rooms
    private void GenerateEnemies() {

    }

    //TODO: implement! handles spawning logic for objects (power-ups, etc...) in item/object type rooms
    private void GenerateObjects() {

    }

}
