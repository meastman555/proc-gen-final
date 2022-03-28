using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//uses a grammar (see SpawnGrammar.cs) and given enemy types to populate the enemy rooms
//note: this only handles data/procedures about spawning enemies -- things like stats on spawn and enemy behavior must be defined in the enemies themselves
public class EnemySpawner : MonoBehaviour
{
    //encapsulates data about enemies that will be spawned in
    [System.Serializable]
    public struct EnemySpawn {
        public GameObject enemyPrefab;
        public int minPerRoom;
        public int maxPerRoom;
    } 
    [SerializeField] private EnemySpawn[] enemyTypes;

    private SpawnGrammar sg;

    //Awake vs Start so that the spawner can be initialized before RoomGeneration kicks off the process
    void Awake() {
        sg = GetComponent<SpawnGrammar>();
    }

    //kick starts enemy generation process, called from RoomGenerator.cs
    //only public function
    public void GenerateAllEnemies(GameObject enemyRoomContainer) {
        //if no enemy room type is defined in grammar, this will not run, so enemies won't be spawned (and game won't crash)
        if(enemyRoomContainer != null) {
            SpawnAllEnemies(enemyRoomContainer.transform);
        }
    }

    private void SpawnAllEnemies(Transform enemyRoomContainer) {
        foreach(Transform room in enemyRoomContainer) {
            SpawnEnemiesInRoom(room);
        }
    }

    private void SpawnEnemiesInRoom(Transform room) {
        //determine this room's enemy type
        int enemyIndex = Random.Range(0, enemyTypes.Length);
        EnemySpawn es = enemyTypes[enemyIndex];

        //determine how many to spawn based on type
        int spawnNum = Random.Range(es.minPerRoom, es.maxPerRoom);
        for(int i = 0; i < spawnNum; i++) {
            GameObject instantiatedEnemy = SpawnEnemyOfType(room.position, es);
            instantiatedEnemy.transform.SetParent(room);
        }
        room.GetComponent<RoomData>().numEnemies = spawnNum;
    }
    private GameObject SpawnEnemyOfType(Vector3 centerOfRoom, EnemySpawn es) {
        string englishPos = sg.GetEnglishPosition();
        //enemy starts in center of room, then moves around based on how the grammar affects it
        Vector3 enemyPos = sg.ConvertEnglishToWorldPos(englishPos, centerOfRoom);
        GameObject instantiatedEnemy = Instantiate(es.enemyPrefab, enemyPos, Quaternion.identity);
        return instantiatedEnemy;
    }
}
