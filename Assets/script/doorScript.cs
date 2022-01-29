using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{
    private int nbrPlayers;

    private bool isCollisionP1;
    private bool isCollisionP2;

    public GameObject player1;
    public GameObject player2;
    
    void Start()
    {
        nbrPlayers = 0;
        isCollisionP1 = false;
        isCollisionP1 = false;
    }

    void Update()
    {
        if (isCollisionP1 && Input.GetKeyDown(KeyCode.Z))
        {
            player1.SetActive(false);
            nbrPlayers += 1;
        }
        else if (isCollisionP2 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            player2.SetActive(false);
            nbrPlayers += 1;
        }

        if (!player1.activeSelf && Input.GetKeyDown(KeyCode.S))
        {
            player1.SetActive(true);
            nbrPlayers -= 1;
        }
        if (!player2.activeSelf && Input.GetKeyDown(KeyCode.DownArrow))
        {
            player2.SetActive(true);
            nbrPlayers -= 1;
        }
        
        //End Game
        if (nbrPlayers == 2)
        {
            Debug.Log("Level finished");
        }
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.name)
        {
            case "player1":
                isCollisionP1 = true;
                break;
            case "player2":
                isCollisionP2 = true;
                break;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        switch (col.gameObject.name)
        {
            case "player1":
                isCollisionP1 = false;
                break;
            case "player2":
                isCollisionP2 = false;
                break;
        }
    }
}