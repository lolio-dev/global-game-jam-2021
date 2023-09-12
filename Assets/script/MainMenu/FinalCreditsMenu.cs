using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using UnityConstants;

public class FinalCreditsMenu : MonoBehaviour
{
    [Header("Child references")]

    [Tooltip("Button to go back to main menu")]
    public Button buttonMainMenu;

    [Tooltip("Button to exit game")]
    public Button buttonExit;


    private void Awake()
    {
        buttonMainMenu.onClick.AddListener(GoBackToMainMenu);
        buttonExit.onClick.AddListener(ExitGame);

        buttonMainMenu.Select();
    }

    private void OnDestroy()
    {
        if (buttonMainMenu)
        {
            buttonMainMenu.onClick.RemoveAllListeners();
        }
    }

    private void GoBackToMainMenu()
    {
        SceneManager.LoadScene((int) ScenesEnum.MainMenu);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
