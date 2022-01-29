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

    void Update()
    {
        if (nbrPlayers == 2)
        {
            Debug.Log("Level finished");
        }
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "player1")
        {
            Debug.Log("Player 1 : Level Finished");
            col.gameObject.SetActive(false);
            nbrPlayers += 1;
        }
        else if (col.gameObject.name == "player2")
        {
            Debug.Log("Player 2 : Level Finished");
            col.gameObject.SetActive(false);
            nbrPlayers += 1;
        }
    }
}