using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WangTiles : MonoBehaviour
{
    [SerializeField] private int maxRecursionDepth;

    [Header("Starting Parameters")]
    [SerializeField] private GameObject startingRoomPrefab;
    //middle of grid is not (0,0), it's (width/2, height/2)
    [SerializeField] private int startX;
    [SerializeField] private int startY;

    [Header("Available Rooms")]
    [SerializeField] private GameObject[] upOpeningRoomPrefabs;
    [SerializeField] private GameObject[] leftOpeningRoomPrefabs;
    [SerializeField] private GameObject[] rightOpeningRoomPrefabs;
    [SerializeField] private GameObject[] downOpeningRoomPrefabs;

    [HideInInspector]
    public class Direction {
        public int dx;
        public int dy;
        public GameObject[] roomsWithComplementaryOpening;

        public Direction(int x, int y, GameObject[] rooms) {
            dx = x;
            dy = y;
            roomsWithComplementaryOpening = rooms;
        }
    }

    private int layoutWidth;
    private int layoutHeight;
    private int adjustedStartX;
    private int adjustedStartY;

    private Direction upDir;
    private Direction leftDir;
    private Direction rightDir;
    private Direction downDir;


    // Start is called before the first frame update
    void Start()
    {
        adjustedStartX = startX + 1;
        adjustedStartY = startY + 1;

        upDir = new Direction(0, -1, downOpeningRoomPrefabs);
        leftDir = new Direction(-1, 0, rightOpeningRoomPrefabs);
        rightDir = new Direction(1, 0, leftOpeningRoomPrefabs);
        downDir = new Direction(0, 1, upOpeningRoomPrefabs);
    }

    public void GenerateRooms(Transform roomContainer, GameObject[,] rooms) {
        layoutWidth = rooms.GetLength(0);
        layoutHeight = rooms.GetLength(1);
        GameObject startRoom = PlaceStartRoom(roomContainer, rooms);
        RecursivelyGenerateNextRoom(roomContainer, rooms, 1, startRoom, adjustedStartX, adjustedStartY);
    }

    private GameObject PlaceStartRoom(Transform roomContainer, GameObject[,] rooms) {
        GameObject instantiatedStartingRoom = Instantiate(startingRoomPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        instantiatedStartingRoom.transform.SetParent(roomContainer);
        rooms[adjustedStartX, adjustedStartY] = instantiatedStartingRoom;
        return instantiatedStartingRoom;
    }

    private void RecursivelyGenerateNextRoom(Transform roomContainer, GameObject[,] rooms, int currentDepth, GameObject currentRoom, int currentX, int currentY)  {
        if(currentDepth > maxRecursionDepth) {
            //PlaceEndRoom(currentRoom, currentX, currentY);
            return;
        }

        RoomData rd = currentRoom.GetComponent<RoomData>();
        if(rd.upOpening) {
            if((currentY - 1 > 0) && !rooms[currentX, currentY - 1]) {
                //GameObject instantiatedRoom = pickAndPlaceRoom(downOpeningRooms, firstFit, currentRoom, currentX, currentY, 0, -1);
                //recursivelyGenerateNextRoom(currentDepth + 1, instantiatedRoom, currentX, currentY - 1);
            }
            else if(!rooms[currentX, currentY - 1]) {
                //placeEndingRoom(currentRoom, currentX, currentY);
            }
        }

        if(rd.leftOpening) {
            if((currentX - 1 > 0) && !rooms[currentX - 1, currentY]) {
                //GameObject instantiatedRoom = pickAndPlaceRoom(rightOpeningRooms, firstFit, currentRoom, currentX, currentY, -1, 0);
                //recursivelyGenerateNextRoom(currentDepth + 1, instantiatedRoom, currentX - 1, currentY);
            }
            else if(!rooms[currentX - 1, currentY])  {
                //placeEndingRoom(currentRoom, currentX, currentY);
            }
        }

        if(rd.rightOpening) {
            if((currentX + 1 < layoutWidth - 1) && !rooms[currentX + 1, currentY]) {
                //GameObject instantiatedRoom = pickAndPlaceRoom(leftOpeningRooms, firstFit, currentRoom, currentX, currentY, 1, 0);
                //recursivelyGenerateNextRoom(currentDepth + 1, instantiatedRoom, currentX + 1, currentY);
            }
            else if(!rooms[currentX + 1, currentY]) {
                //placeEndingRoom(currentRoom, currentX, currentY);
            }
        }

        if(rd.downOpening) {
            if((currentY + 1 < layoutHeight - 1) && !rooms[currentX, currentY + 1]) {
                //GameObject instantiatedRoom = pickAndPlaceRoom(upOpeningRooms, firstFit, currentRoom, currentX, currentY, 0, 1);
                //recursivelyGenerateNextRoom(currentDepth + 1, instantiatedRoom, currentX, currentY + 1);
            }
            else if(!rooms[currentX, currentY + 1]) {
                //placeEndingRoom(currentRoom, currentX, currentY);
            }
        }
    }
}