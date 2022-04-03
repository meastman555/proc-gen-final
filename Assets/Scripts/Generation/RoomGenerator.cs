using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private Transform roomContainer;
    //X max
    [SerializeField] private int width;
    //Y max
    [SerializeField] private int height;    
    //middle of grid is not (0,0), it's (width/2, height/2)
    [SerializeField] private int startX;
    [SerializeField] private int startY;
    [SerializeField] private int minRoomCount;

    private int adjustedWidth;
    private int adjustedHeight;
    private int adjustedStartX;
    private int adjustedStartY;

    private WangTiles wang;
    private RoomGrammar grammar;
    private EnemySpawner enemySpawner;
    private ItemSpawner itemSpawner;


    //internal storage of rooms, adds in a buffer so need to adjust some variables
    //TODO: move this to just be within wang? So far it's not used anywhere else
    //however, if say the grammar wanted to know what surrounding room types are before deciding the next room's (to prevent them from being the same, etc.) then it would be used, so wait on this change
    private GameObject[,] rooms;
 
    void Start() {
        wang = GetComponent<WangTiles>();
        grammar = GetComponent<RoomGrammar>();
        enemySpawner = GetComponent<EnemySpawner>();
        itemSpawner = GetComponent<ItemSpawner>();

        adjustedWidth = width + 2;
        adjustedHeight = height + 2;
        adjustedStartX = startX + 1;
        adjustedStartY = startY + 1;

        //this is the all important call that generates the entire level once the scene is loaded
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
        GenerateItems();
        //TODO: add any other needed generation layers!
    }

    //generates just the blank tiles for structure -- uses depth-first recursive backtracking Wang Tile implementation with both first-fit and best-fit tile algorithms
    //rooms is passed by reference to wang tiles, and it's where the final layout ends up internally, in the editor they're subdivided under roomContainer object
    private void GenerateBaseLayout() {
        wang.GenerateRooms(roomContainer, rooms, adjustedStartX, adjustedStartY);
        //if wang doesn't generate enough rooms, restart and repeat until it does
        if(roomContainer.childCount < minRoomCount) {
            ResetAll();
        }
    }

    //passes all the blank tiles through the room type grammar
    //linearly assigns the room types with no context of surrounding rooms' types
    private void PassRoomsThroughGrammar() {
        grammar.AssignRoomTypes(roomContainer);
    }

    //handles spawning logic for enemies in the enemy type rooms
    private void GenerateEnemies() {
        //from grammar generation, if there is a room type of "Enemy" this container object is guaranteed to exist
        //if it does not, GenerateAllEnemies won't do anything
        GameObject enemyRoomContainer = GameObject.Find("EnemyRooms");
        enemySpawner.GenerateAllEnemies(enemyRoomContainer);
    }

    //handles spawning logic for items (power-ups, health pickups, etc.) in item type rooms
    private void GenerateItems() {
        //from grammar generation, if there is a room type of "Item" this container object is guaranteed to exist
        //if it does not, GenerateAllEnemies won't do anything
        GameObject itemRoomContainer = GameObject.Find("ItemRooms");
        itemSpawner.GenerateAllItems(itemRoomContainer);
    }
}
