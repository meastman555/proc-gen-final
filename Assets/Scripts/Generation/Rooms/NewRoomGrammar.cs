using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRoomGrammar : MonoBehaviour
{
    //encapsulates data pertaining to a roomtype, all editor-specified
    [System.Serializable]
    public struct RoomType {
        public string name;
        [Range(0, 100)] public int chance;
        //TODO: remove once more art is in the game, this is just for debug
        public Color color;
    }
    [SerializeField] private RoomType[] roomTypes;

    //stores strings corresponding to their chance of being randomly picked
    //for example, if "enemy" has a 70% chance of being chosen in grammar, the string "enemy" appears 70 times
    private List<string> roomTypesGrammar;
    //makes for easy lookup, avoid a "find" function
    private Dictionary<string, RoomType> roomTypesDict;
    
    //since sub-containers will be created to store rooms of each type (for organization and later spawning), keep separate reference of actual rooms
    private List<GameObject> allRooms;
    private List<GameObject> subContainers;

    //Awake vs Start so that the grammar can be initialized before they're attempted to be used when RoomGeneration kicks off the process
    void Awake()
    {
        roomTypesGrammar = new List<string>();
        roomTypesDict = CreateDict();
        allRooms = new List<GameObject>();
        subContainers = new List<GameObject>();

        //TODO: It's a feature not a bug! If use enters say, 100% chance for two types, that will just cause them each to have 100 entries
        foreach(RoomType rt in roomTypes) {
            for(int i = 0; i < rt.chance; i++) {
                roomTypesGrammar.Add(rt.name);
            }
        }
    }

    //see comment above roomTypesDict variable
    private Dictionary<string, RoomType> CreateDict() {
        Dictionary<string, RoomType> temp = new Dictionary<string, RoomType>();
        foreach(RoomType rt in roomTypes) {
            temp.Add(rt.name, rt);
        }
        return temp;
    }

    //kicks off the placement of roomtypes -- this is called in RoomGenerator.cs
    //the only public function
    public void AssignRoomTypes(Transform roomContainer) {
        CaptureAllRooms(roomContainer);
        CreateTypeContainers(roomContainer);
        AssignAllRoomsAndOrganize();
    }

    //see comment above allRooms variable
    private void CaptureAllRooms(Transform roomContainer) {
        foreach(Transform child in roomContainer) {
            allRooms.Add(child.gameObject);
        }
    } 

    //subcontainers exist solely to stay organized and the make look-up of rooms based on type more efficient
    //extra set-up now for less work later    
    private void CreateTypeContainers(Transform roomContainer) {
        foreach(RoomType rt in roomTypes) {
            string typeContainerName = rt.name + "Rooms";
            GameObject typeContainer = new GameObject(typeContainerName);
            typeContainer.transform.SetParent(roomContainer);
            subContainers.Add(typeContainer);
        }
    }

    //TODO: find something more efficient than this to organize them? trying to avoid O(N) find 
    private void AssignAllRoomsAndOrganize() {
        foreach(GameObject room in allRooms) {
            AssignSingleRoom(room);
            PutInCorrectSubContainer(room);
        }
    }

    //pulls from the grammar to give this room a type!
    //type will dictate what happens to the room in the next layers of generation
    private void AssignSingleRoom(GameObject room) {
        int grammarIndex = Random.Range(0, roomTypesGrammar.Count);
        string rtName = roomTypesGrammar[grammarIndex];
        RoomType rt = roomTypesDict[rtName];

        room.GetComponent<SpriteRenderer>().color = rt.color;
        room.GetComponent<RoomData>().roomTypeName = rtName;
    }

    //puts the room into the corresponding subcontainer based on type
    //assumes rd has already has the type correctly set
    private void PutInCorrectSubContainer(GameObject room) {
        RoomData rd = room.GetComponent<RoomData>();
        //TODO: ahhhhhhhhhhh ! lol
        string hackName = rd.roomTypeName + "Rooms";
        foreach(GameObject sc in subContainers) {
            if(hackName == sc.name) {
                room.transform.SetParent(sc.transform);
                break;
            }
        }
    }
}
