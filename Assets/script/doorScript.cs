using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{
    private int nbrPlayers;

    void Start()
    {
        nbrPlayers = 0;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var playerMovement = col.GetComponent<playerMovement>();
        if (playerMovement)
        {
            playerMovement.RegisterNearbyDoor(this);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var playerMovement = col.GetComponent<playerMovement>();
        if (playerMovement)
        {
            playerMovement.UnregisterNearbyDoor(this);
        }
    }

    public void MakeCharacterEnterDoor(playerMovement playerMovementScript)
    {
        playerMovementScript.OnEnterDoor(this);
        nbrPlayers += 1;

        CheckCharactersInDoor();
    }

    public void MakeCharacterExitDoor(playerMovement playerMovementScript)
    {
        playerMovementScript.OnExitDoor(this);
        nbrPlayers -= 1;
    }

    private void CheckCharactersInDoor()
    {
        //End Game
        if (nbrPlayers == 2)
        {
            Debug.Log("Level finished");
        }
    }
}