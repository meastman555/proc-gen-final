using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//uses a grammar (see SpawnGrammar.cs) and given item types to populate the item rooms
//note: this only handles data/procedures about spawning items -- things like effects and behavior on pick-up need to be defined in the items themselves
public class ItemSpawner : MonoBehaviour
{
    //so far, only one item of one type is spawned per item room
    //TODO: add a struct (similar to EnemySpawn) if item generation should get more complex
    [SerializeField] private GameObject[] itemTypes;

    private SpawnGrammar sg;

    void Awake() {
        sg = GetComponent<SpawnGrammar>();
    }

    //kick starts item generation process, called from RoomGenerator.cs
    //only public function
    public void GenerateAllItems(GameObject itemRoomContainer) {
        //if no item room type is defined in grammar, this will not run, so items will not be generated and game will not crash
        if(itemRoomContainer != null) {
            SpawnAllItems(itemRoomContainer.transform);
        }
    }

    private void SpawnAllItems(Transform itemRoomContainer) {
        foreach(Transform room in itemRoomContainer) {
            //TODO: change if multiple items (maybe of multiple types) can be generated (would match enemy generation more)
            GameObject instantiatedItem = SpawnItemInRoom(room);
            instantiatedItem.transform.SetParent(room);
            room.GetComponent<RoomData>().numItems = 1;
        }
    }

    //given a room, picks an item type to spawn, and uses the grammar to determine location in room
    private GameObject SpawnItemInRoom(Transform room) {
        //determine this room's item type
        int itemIndex = Random.Range(0, itemTypes.Length);
        GameObject itemPrefab = itemTypes[itemIndex];

        string englishPos = sg.GetEnglishPosition();
        //item starts in center of room, then moves around based on how the grammar affects it
        Vector3 centerOfRoom = room.transform.position;
        Vector3 itemPos = sg.ConvertEnglishToWorldPos(englishPos, centerOfRoom);
        GameObject instantiatedItem = Instantiate(itemPrefab, itemPos, Quaternion.identity);
        return instantiatedItem;
    }
}
