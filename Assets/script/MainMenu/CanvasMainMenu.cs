using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// Script to attach to Canvas Main Menu
/// Tag: CanvasMainMenu
public class CanvasMainMenu : MonoBehaviour
{
    [Header("Child references")]

    [Tooltip("Title Text (TMP)")]
    public TextMeshProUGUI titleText;

    [Tooltip("Menus Parent")]
    public Transform menusParent;


    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowTitle()
    {
        // Show game object completely so it affects children too, on which we play some animation
        titleText.gameObject.SetActive(true);
    }

    public void HideTitle()
    {
        // Hide game object completely so it affects children too, on which we play some animation
        titleText.gameObject.SetActive(false);
    }
}
