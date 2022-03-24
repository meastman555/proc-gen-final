using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tracks any and all user input during the game, excluding anything GUI related
//uses delegates to have the actual logic be handled in the classes it needs to be -- this is just the manager
//this leads it to be super modular and functionality can be easily added in
public class UserInput : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject roomGeneration;

    private PlayerMovement playerMove;
    private PlayerFire playerFire;
    private RoomGenerator roomGenerator;

    void Start() {
        playerMove = player.GetComponent<PlayerMovement>();
        playerFire = player.GetComponent<PlayerFire>();
        roomGenerator = roomGeneration.GetComponent<RoomGenerator>();
    }

    //TODO: if there are any weird bugs, worth putting physics stuff in FixedUpdate and separating that from the input (through booleans or states)
    void Update() {
        CheckGenerationReset();
        UpdatePlayerRotation();
        CheckPlayerMovement();
        CheckPlayerFire();
        //TODO: check other debug things and potentially a pause menu, etc.
    }

    private void CheckGenerationReset() {
        if(Input.GetKeyDown(KeyCode.R)) {
            roomGenerator.Reset();
        }
    }
    private void UpdatePlayerRotation() {
        playerFire.AimAtMouse();
    }

    private void CheckPlayerFire() {
        if(Input.GetMouseButtonDown(0)) {
            playerFire.FireShot();
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
