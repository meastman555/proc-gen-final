using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: refactor more! still some things that could be cleaned up
//TODO: fix the generation bug (still not too sure why it's happening) 
public class WangTiles : MonoBehaviour
{
    [Header("Generation Parameters")]
    // [SerializeField] private int maxRecursionDepth;
    [SerializeField] private int maxFitTries;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;

    [Header("Starting Parameters")]
    [SerializeField] private GameObject startingRoomPrefab;

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

    private int maxRecursionDepth;
    private int layoutWidth;
    private int layoutHeight;
    private Direction upDir;
    private Direction leftDir;
    private Direction rightDir;
    private Direction downDir;

    //Awake vs Start so that these directions can be initialized before they're attempted to be used when RoomGeneration kicks off the process
    void Awake()
    {
        upDir = new Direction(0, -1, downOpeningRoomPrefabs);
        leftDir = new Direction(-1, 0, rightOpeningRoomPrefabs);
        rightDir = new Direction(1, 0, leftOpeningRoomPrefabs);
        downDir = new Direction(0, 1, upOpeningRoomPrefabs);
    }

    //kicks off the entire generation of a blank layout -- this is called in RoomGenerator.cs
    //the only public function
    public void GenerateRooms(Transform roomContainer, GameObject[,] rooms, int startX, int startY) {
        maxRecursionDepth = InitialSettingsSingleton.Instance.GetMaxRecursionDepth();
        layoutWidth = rooms.GetLength(0);
        layoutHeight = rooms.GetLength(1);
        GameObject startRoom = PlaceStartRoom(roomContainer, rooms, startX, startY);
        RecursivelyGenerateNextRoom(roomContainer, rooms, 1, startRoom, startX, startY);
    }

    private GameObject PlaceStartRoom(Transform roomContainer, GameObject[,] rooms, int startX, int startY) {
        GameObject instantiatedStartingRoom = Instantiate(startingRoomPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        instantiatedStartingRoom.transform.SetParent(roomContainer);
        rooms[startX, startY] = instantiatedStartingRoom;
        return instantiatedStartingRoom;
    }

    //recursively picks and places rooms in a depth-first manner
    //general rules:
    //-- if the next placed room would be outside the defined layout size, that is ok, just place a room with ONLY the complementary opening (no additional branches) and thus end recursion on that path
    //-- if the next placed room would be in a spot that already has a room (multiple shared openings that create a loop), don't place anything and move on (end recursion)
    //-- if this iteration would exceed the specified recursion depth, don't do anything
    //the way the indecies and bounds are set up, there should never be an out of bound error on the array, but I'm not using assert statements nor proofs so this is more anecdoatal. find a way to guarantee?
    private void RecursivelyGenerateNextRoom(Transform roomContainer, GameObject[,] rooms, int currentDepth, GameObject currentRoom, int currentX, int currentY)  {
        if(currentDepth > maxRecursionDepth) {
            PlaceEndRooms(roomContainer, rooms, currentRoom, currentX, currentY);
            //empty return breaks recursion on this branch
            return;
        }

        //checkes each opening of the current room independently and kicks off recursion if the next room is in bounds and there is an available spot
        //if the next room would go out of bounds, but there isn't one there, end the branch by placing a dead-end ending room
        RoomData rd = currentRoom.GetComponent<RoomData>();
        if(rd.upOpening) {
            //in bounds and there isn't a room
            if((currentY - 1 > 0) && !rooms[currentX, currentY - 1]) {
                GameObject instantiatedUpRoom = PickAndPlaceNextRoom(roomContainer, rooms, currentRoom, currentX, currentY, upDir, FirstFit);
                RecursivelyGenerateNextRoom(roomContainer, rooms, currentDepth + 1, instantiatedUpRoom, currentX, currentY - 1);
            }
            //would be placed out of bounds, and there isn't a room (so cap this branch off)
            else if(!rooms[currentX, currentY - 1]) {
                PlaceEndRooms(roomContainer, rooms, currentRoom, currentX, currentY);
            }
        }

        if(rd.leftOpening) {
            //in bounds and there isn't a room
            if((currentX - 1 > 0) && !rooms[currentX - 1, currentY]) {
                GameObject instantiatedLeftRoom = PickAndPlaceNextRoom(roomContainer, rooms, currentRoom, currentX, currentY, leftDir, FirstFit);
                RecursivelyGenerateNextRoom(roomContainer, rooms, currentDepth + 1, instantiatedLeftRoom, currentX - 1, currentY);
            }
            //would be placed out of bounds, and there isn't a room (so cap this branch off)
            else if(!rooms[currentX - 1, currentY])  {
                PlaceEndRooms(roomContainer, rooms, currentRoom, currentX, currentY);
            }
        }

        if(rd.rightOpening) {
            //in bounds and there isn't a room
            if((currentX + 1 < layoutWidth - 1) && !rooms[currentX + 1, currentY]) {
                GameObject instantiatedRightRoom = PickAndPlaceNextRoom(roomContainer, rooms, currentRoom, currentX, currentY, rightDir, FirstFit);
                RecursivelyGenerateNextRoom(roomContainer, rooms, currentDepth + 1, instantiatedRightRoom, currentX + 1, currentY);
            }
            //would be placed out of bounds, and there isn't a room (so cap this branch off)
            else if(!rooms[currentX + 1, currentY]) {
                PlaceEndRooms(roomContainer, rooms, currentRoom, currentX, currentY);
            }
        }

        if(rd.downOpening) {
            //in bounds and there isn't a room
            if((currentY + 1 < layoutHeight - 1) && !rooms[currentX, currentY + 1]) {
                GameObject instantiatedDownRoom = PickAndPlaceNextRoom(roomContainer, rooms, currentRoom, currentX, currentY, downDir, FirstFit);
                RecursivelyGenerateNextRoom(roomContainer, rooms, currentDepth + 1, instantiatedDownRoom, currentX, currentY + 1);
            }
            //would be placed out of bounds, and there isn't a room (so cap this branch off)
            else if(!rooms[currentX, currentY + 1]) {
                PlaceEndRooms(roomContainer, rooms, currentRoom, currentX, currentY);
            }
        }
    }

    //encapsulates the two-step process for simplicity, since lots of parameters overlap
    private GameObject PickAndPlaceNextRoom(Transform roomContainer, GameObject[,] rooms, GameObject currentRoom, int currentX, int currentY, Direction placeDir, System.Func<GameObject[,], GameObject, int, int, bool> FitFunction) {
        GameObject nextRoomPrefab = PickRoom(rooms, FitFunction, currentX, currentY, placeDir);
        GameObject instatntiatedRoom = PlaceNextRoom(roomContainer, rooms, nextRoomPrefab, currentRoom, currentX, currentY, placeDir);
        return instatntiatedRoom;
    }

    //picks a random room from those specified (in placeDir) until the fit condition is satisfied
    private GameObject PickRoom(GameObject[,] rooms, System.Func<GameObject[,], GameObject, int, int, bool> FitFunction, int currentX, int currentY, Direction placeDir) {
        int dx = placeDir.dx;
        int dy = placeDir.dy;
        GameObject[] possibleRooms = placeDir.roomsWithComplementaryOpening;
        
        int roomIndex = Random.Range(0, possibleRooms.Length);
        GameObject nextRoomPrefab = possibleRooms[roomIndex];
        int currentFitTries = 0;
        //this will break if it takes too long to find a room, though there is currently a bug where a correct room is never found, which shouldn't be possible!
        //TODO: fix the bug -- removing the tries check will cause it to infinitely loop
        while(!FitFunction(rooms, nextRoomPrefab, currentX + dx, currentY + dy) && currentFitTries < maxFitTries) {
            currentFitTries++;
            roomIndex = Random.Range(0, possibleRooms.Length);
            nextRoomPrefab = possibleRooms[roomIndex];
        }

        return nextRoomPrefab;
    }
    
    //given a potential room, immediately pick it as the next if it complements neighbor openings, does not care about any new branches it might create
    //nextRoomX and nextRoomY are guaranteed to be in bounds of rooms array, since that gets checked before this method is called
    //TODO: are the +-1 on the nextRoom coords guaranteed to be in bounds? I belive they are but can't prove 100%
    //returns false if any misalignments are found, true if it passes all of them -- have to check both ways for any neighbor
    private bool FirstFit(GameObject[,] rooms, GameObject potentialRoomPrefab, int nextRoomX, int nextRoomY) {
        RoomData rd = potentialRoomPrefab.GetComponent<RoomData>();
        //check up neighbor
        if(rooms[nextRoomX, nextRoomY - 1]) {
            RoomData upNeighborRD = rooms[nextRoomX, nextRoomY - 1].GetComponent<RoomData>();
            if((rd.upOpening && !upNeighborRD.downOpening) || (upNeighborRD.downOpening && !rd.upOpening)) {
                return false;
            }
        }
        //check left neighbor
        if(rooms[nextRoomX - 1, nextRoomY]) {
            RoomData leftNeighborRD = rooms[nextRoomX - 1, nextRoomY].GetComponent<RoomData>();
            if((rd.leftOpening && !leftNeighborRD.rightOpening) || (leftNeighborRD.rightOpening && !rd.leftOpening)) {
                return false;
            }
        }
        //check right neighbor
        if(rooms[nextRoomX + 1, nextRoomY]) {
            RoomData rightNeighborRD = rooms[nextRoomX + 1, nextRoomY].GetComponent<RoomData>();
            if((rd.rightOpening && !rightNeighborRD.leftOpening) || (rightNeighborRD.leftOpening && !rd.rightOpening)) {
                return false;
            }
        }
        //check down neighbor
        if(rooms[nextRoomX, nextRoomY + 1]) {
            RoomData downNeighborRD = rooms[nextRoomX, nextRoomY + 1].GetComponent<RoomData>();
            if((rd.downOpening && !downNeighborRD.upOpening) || (downNeighborRD.upOpening && !rd.downOpening)) {
                return false;
            }
        }
        //since this is used for regular branching, where we want it to continue generating, make sure there is more than one opening
        //TODO: though, this may be causing the bug in generation?
        bool moreThanOneOpening = rd.numOpenings > 1;
        return moreThanOneOpening;
    }

    //ensures that all the surrounding openings match, but there are no additional openings, so branching can end
    //I *think* nextRoomX and nextRoomY are guaranteed to be in bounds as per comment in firstFit, but no formal asserts or proving so I'm not 100% sure
    //TODO: are the +-1 on the nextRoom coords guaranteed to be in bounds? I belive they are but can't prove 100%
    //returns false if any misalignments are found, true if it passes all of them -- have to check both ways for any neighbor, and also that the number of openings match exactly
    private bool BestFit(GameObject[,] rooms, GameObject potentialRoomPrefab, int nextRoomX, int nextRoomY) {
        RoomData rd = potentialRoomPrefab.GetComponent<RoomData>();
        int numSurroundingOpenings = 0;
        //check up neighbor
        if(rooms[nextRoomX, nextRoomY - 1]) {
            RoomData upNeighborRD = rooms[nextRoomX, nextRoomY - 1].GetComponent<RoomData>();
            if(upNeighborRD.downOpening) {
                numSurroundingOpenings++;
            }
            if((rd.upOpening && !upNeighborRD.downOpening) || (upNeighborRD.downOpening && !rd.upOpening)) {
                return false;
            }
        }
        //check left neighbor
        if(rooms[nextRoomX - 1, nextRoomY]) {
            RoomData leftNeighborRD = rooms[nextRoomX - 1, nextRoomY].GetComponent<RoomData>();
            if(leftNeighborRD.rightOpening) {
                numSurroundingOpenings++;
            }
            if((rd.leftOpening && !leftNeighborRD.rightOpening) || (leftNeighborRD.rightOpening && !rd.leftOpening)) {
                return false;
            }
        }
        //check right neighbor
        if(rooms[nextRoomX + 1, nextRoomY]) {
            RoomData rightNeighborRD = rooms[nextRoomX + 1, nextRoomY].GetComponent<RoomData>();
            if(rightNeighborRD.leftOpening) {
                numSurroundingOpenings++;
            }
            if((rd.rightOpening && !rightNeighborRD.leftOpening) || (rightNeighborRD.leftOpening && !rd.rightOpening)) {
                return false;
            }
        }
        //check down neighbor
        if(rooms[nextRoomX, nextRoomY + 1]) {
            RoomData downNeighborRD = rooms[nextRoomX, nextRoomY + 1].GetComponent<RoomData>();
            if(downNeighborRD.upOpening) {
                numSurroundingOpenings++;
            }
            if((rd.downOpening && !downNeighborRD.upOpening) || (downNeighborRD.upOpening && !rd.downOpening)) {
                return false;
            }
        }
        bool allOpeningsMatch = rd.numOpenings == numSurroundingOpenings;
        return allOpeningsMatch;
    }

    //places a new room in relation to the current room given a prefab, coordinates, and direction (dx, dy)
    private GameObject PlaceNextRoom(Transform roomContainer, GameObject[,] rooms, GameObject nextRoomPrefab, GameObject currentRoom, int currentX, int currentY, Direction placeDir) {
        Vector3 nextRoomPos = currentRoom.transform.position;
        nextRoomPos.x += (xOffset * placeDir.dx);
        //dy is negated because moving up in the rooms array (dy = -1) actually moves positively up in world space -- and vice-versa -- so flip
        nextRoomPos.y += (yOffset * -placeDir.dy);
        GameObject instantiatedRoom = Instantiate(nextRoomPrefab, nextRoomPos, Quaternion.identity);

        //important to correctly update the editor parent and internal array with this new instantiated room, not the prefab
        instantiatedRoom.transform.SetParent(roomContainer);
        rooms[currentX + placeDir.dx, currentY + placeDir.dy] = instantiatedRoom;

        return instantiatedRoom;
    }

    //places an ending room -- only closes complementary neighbor openings, does not lead to any new branches, by using BestFit to match openings exactly
    private void PlaceEndRooms(Transform roomContainer, GameObject[,] rooms, GameObject currentRoom, int currentX, int currentY) {
        RoomData rd = currentRoom.GetComponent<RoomData>();
        //checks each direction independently, similar to when recursively generating rooms, to make sure all openings are closed
        if(rd.upOpening && !rooms[currentX, currentY - 1]) {
            PickAndPlaceNextRoom(roomContainer, rooms, currentRoom, currentX, currentY, upDir, BestFit);
        }
        if(rd.leftOpening && !rooms[currentX - 1, currentY]) {
            PickAndPlaceNextRoom(roomContainer, rooms, currentRoom, currentX, currentY, leftDir, BestFit);
        }
        if(rd.rightOpening && !rooms[currentX + 1, currentY]) {
            PickAndPlaceNextRoom(roomContainer, rooms, currentRoom, currentX, currentY, rightDir, BestFit);
        }
        if(rd.downOpening && !rooms[currentX, currentY + 1]) {
            PickAndPlaceNextRoom(roomContainer, rooms, currentRoom, currentX, currentY, downDir, BestFit);
        }
    }
}
