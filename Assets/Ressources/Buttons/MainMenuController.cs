using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public int levelProgress;


    [SerializeField] private Button continueButton;
    void Start()
    {
        levelProgress = PlayerPrefs.GetInt("level");
        if (levelProgress == 0)
        {
            continueButton.interactable = false;
        }
    }

    void Update()
    {
        levelProgress = PlayerPrefs.GetInt("level");
        Debug.Log(levelProgress);
    }
    
    public void NewGame()
    {
        PlayerPrefs.SetInt("level", 1);
        //SceneManager.LoadScene (sceneBuildIndex:level + 1);
    }

    public void Continue()
    {
        Debug.Log("Continue");
        //SceneManager.LoadScene (sceneBuildIndex:level + 1);
    }
}
