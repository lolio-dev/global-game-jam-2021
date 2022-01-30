using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private GameObject escapeCanvas;

    private bool isEscape;
    
    void Start()
    {
        escapeCanvas.SetActive(false);
        isEscape = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isEscape = !isEscape;
        }
        
        if (isEscape == true)
        {
            Time.timeScale = 0;
            escapeCanvas.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            escapeCanvas.SetActive(false);
        }
    }

    public void Continue()
    {
        isEscape = false;
    }

    public void Menu()
    {
        //SceneManager.LoadScene (sceneBuildIndex:1);
        Debug.Log("Menu");
    }
}
