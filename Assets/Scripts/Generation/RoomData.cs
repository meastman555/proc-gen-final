using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//simple data storage class that is attached to each room prefab
//needs to be public so room generator stuff can easily access them (and I promise not to change values needlessly in the code :smile:)
public class RoomData : MonoBehaviour
{
    //structure data
    public int numOpenings;
    public bool upOpening;
    public bool leftOpening;
    public bool rightOpening;
    public bool downOpening;

    //type data
    public string roomTypeName;
    public int numEnemies;
    public int numItems;

    //alternate sprites (used to give the room the proper look depending on grammar type)
    //all rooms start out at normal so don't need to track that
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private Sprite itemSprite;

    private SpriteRenderer sp;

    void Awake() {
        sp = GetComponent<SpriteRenderer>();
    }

    //given the current room type update the sprite
    //set by the grammar first, so this works on updated sprite
    //TOD0: this is too static!! rework to work kinda like the room containers stuff?
    //currently need to add string for each new room type in code when a new type is added in editor
    public void UpdateRoomSprite() {
        if(roomTypeName == "Enemy") {
            sp.sprite = enemySprite;
        }
        else if(roomTypeName == "Item") {
            sp.sprite = itemSprite;
        }
    }
}
