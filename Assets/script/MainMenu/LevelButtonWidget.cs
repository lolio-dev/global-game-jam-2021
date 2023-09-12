using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonWidget : MonoBehaviour
{
    [Header("Child references")]

    [Tooltip("Level thumbnail")]
    public Image levelThumbnail;

    [Tooltip("Level text")]
    public TextMeshProUGUI levelText;


    /* Sibling components */

    private Button m_LevelButton;


    /* Main parameters */

    /// Index of level represented by this widget, and that is entered on confirm
    private int m_LevelIndex;


    private void Awake()
    {
        m_LevelButton = GetComponent<Button>();

        // Only register button callback once, in Awake rather than the Init methods,
        // to avoid multiple registration.
        m_LevelButton.onClick.AddListener(OnConfirmLevelSelection);
    }

    private void OnDestroy()
    {
        if (m_LevelButton)
        {
            m_LevelButton.onClick.RemoveAllListeners();
        }
    }

    public void Init(int levelIndex)
    {
        m_LevelIndex = levelIndex;

        RefreshVisual();
    }

    private void RefreshVisual()
    {
        LevelData[] levelDataList = MainMenuManager.Instance.levelDataList.levelDataArray;
        LevelData levelData = levelDataList[m_LevelIndex];

        if (levelThumbnail != null)
        {
            levelThumbnail.sprite = levelData.thumbnail;
            levelThumbnail.enabled = true;
        }

        // Level index starts at 0, so +1 to make it start at 1 (human-readable)
        levelText.text = $"Level {m_LevelIndex + 1}";
    }

    public void Select()
    {
        m_LevelButton.Select();
    }

    private void OnConfirmLevelSelection()
    {
        // Start selected level
        MainMenuManager.Instance.StartLevel(m_LevelIndex);
    }
}
