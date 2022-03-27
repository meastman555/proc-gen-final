using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a small grammer that is used to translate english positions to in-game coordiantes
public class SpawnGrammar : MonoBehaviour
{

    [SerializeField] private float roomWidth;
    [SerializeField] private float roomHeight;

    private float widthStepSize;
    private float heightStepSize;

    private string[] vertical;
    private string[] horizontal;
    private string[][] grammar;

    //Awake to ensure this grammar gets initialized before it's needed
    void Awake() {
        widthStepSize = roomWidth / 3.0f;
        heightStepSize = roomHeight / 3.0f;

        vertical = new string[] {"top", "middle", "bottom"};
        horizontal = new string[] {"left", "middle", "right"};
        //jagged array stuff to get around weird C# compiling requirements
        grammar = new string[][] {vertical, horizontal};

    }

    //returns an english position from the grammar
    //TODO: there might be a better way to do this? the top level grammar array seems kind of unnecessary, unless another element like just "center" is added
    public string GetEnglishPosition() {
        int verticalIndex = Random.Range(0, 3);
        int horizontalIndex = Random.Range(0, 3);

        string verticalString = grammar[0][verticalIndex];
        string horizontalString = grammar[1][horizontalIndex];

        string placement = verticalString + " " + horizontalString;
        return placement;
    }

    //given a string (from the grammar above), change pos to reflect the conversion into world space
    public Vector3 ConvertEnglishToWorldPos(string eng, Vector3 pos) {
        Vector3 updatedPos = pos;
        //since there are only 9 possibilities for the grammar, went ahead and hard-coded them in the switch
        //it proved to be more readable then doing it one at a time programatically (will change if more gets added to grammar)
        //uses step sizes to push the spawn position around within the room
        switch(eng) {
            case "top left": {
                updatedPos.x -= widthStepSize;
                updatedPos.y += heightStepSize;
                break;
            }
            case "top middle": {
                updatedPos.y += heightStepSize;
                break;
            }
            case "top right": {
                updatedPos.x += widthStepSize;
                updatedPos.y += heightStepSize;
                break;
            }
            case "middle left": {
                updatedPos.x -= widthStepSize;
                break;
            }
            case "middle middle": {
                //start at center, end at center, so do nothing!
                break;
            }
            case "middle right": {
                updatedPos.x += widthStepSize;
                break;
            }
            case "bottom left": {
                updatedPos.x -= widthStepSize;
                updatedPos.y -= heightStepSize;
                break;
            }
            case "bottom middle": {
                updatedPos.y -= heightStepSize;
                break;
            }
            case "bottom right": {
                updatedPos.x += widthStepSize;
                updatedPos.y -= heightStepSize;
                break;
            }
            default: {
                Debug.Log("SpawnGrammar does not recognize this string: " + eng);
                break;
            }
        }
        return updatedPos;
    }
}
