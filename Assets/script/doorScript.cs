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
        if (isCollisionP1 == true && Input.GetKeyDown(KeyCode.Z))
        {
            player1.SetActive(false);
            nbrPlayers += 1;
        }
        else if (isCollisionP2 == true && Input.GetKeyDown(KeyCode.UpArrow))
        {
            player2.SetActive(false);
            nbrPlayers += 1;
        }

        if (!player1.activeSelf && Input.GetKeyDown(KeyCode.S))
        {
            player1.SetActive(true);
        }
        if (!player2.activeSelf && Input.GetKeyDown(KeyCode.DownArrow))
        {
            player2.SetActive(true);
        }
        
        //End Game
        if (nbrPlayers == 2)
        {
            Debug.Log("Level finished");
        }
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "player1")
        {
            isCollisionP1 = true;
        }
        else if (col.gameObject.name == "player2")
        {
            isCollisionP2 = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "player1")
        {
            isCollisionP1 = false;
            nbrPlayers -= 1;
        }
        else if (col.gameObject.name == "player2")
        {
            isCollisionP2 = false;
            nbrPlayers -= 1;
        }
    }
}