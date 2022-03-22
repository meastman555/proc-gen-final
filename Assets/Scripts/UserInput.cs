using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tracks any and all user input during the game
//currently does not include anything in the main menu
//uses delegates to have the actual logic be handled in the classes it needs to be -- this is just the manager
public class UserInput : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private TwoDMovement playerMove;

    void Start() {
        playerMove = player.GetComponent<TwoDMovement>();
    }

    //non-physics sensitive input, such as debug actions
    void Update() {
        
    }

    //physics sensitive input, such as player movement
    void FixedUpdate() {
        CheckPlayerMovement();
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
