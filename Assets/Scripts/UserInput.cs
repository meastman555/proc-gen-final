using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tracks any and all user input during the game
//currently does not include anything in the main menu
//uses delegates to have the actual logic be handled in the classes it needs to be -- this is just the manager
//this leads it to be super modular and functionality can be easily added in
public class UserInput : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject roomGeneration;

    private PlayerMovement playerMove;
    private RoomGenerator roomGenerator;

    void Start() {
        playerMove = player.GetComponent<PlayerMovement>();
        roomGenerator = roomGeneration.GetComponent<RoomGenerator>();
    }

    //non-physics sensitive input, such as debug actions
    void Update() {
        CheckGenerationReset();
        //TODO: check other debug things and potentially a pause menu, etc.
    }

    //physics sensitive input, such as player movement
    void FixedUpdate() {
        CheckPlayerMovement();
        //TODO: check player fire, I think that will end up being physics based
    }

    private void CheckGenerationReset() {
        if(Input.GetKeyDown(KeyCode.R)) {
            roomGenerator.Reset();
        }
    }

    private void CheckPlayerMovement() {
        if(Input.GetKey(KeyCode.W)) {
            playerMove.MoveUp();
        }
        else if(Input.GetKey(KeyCode.S)) {
            playerMove.MoveDown();
        }
        else if(Input.GetKey(KeyCode.A)) {
            playerMove.MoveLeft();
        }
        else if(Input.GetKey(KeyCode.D)) {
            playerMove.MoveRight();
        }
        else {
            playerMove.StopMoving();
        }
    }
}
