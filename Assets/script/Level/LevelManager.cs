// hsandt: extracted and adapted some methods from:
// https://github.com/hsandt/dragon-raid/blob/develop/Assets/Scripts/InGame/InGameManager.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityConstants;
using CommonsPattern;

public class LevelManager : SingletonManager<LevelManager>
{
    [Header("Assets")]

    [Tooltip("Level Data List asset")]
    public LevelDataList levelDataList;

    /// Cached level data, retrieved from the level identifier of the current scene
    private LevelData m_LevelData;

    /// Cached level data, retrieved from the level identifier of the current scene (getter)
    public LevelData LevelData => m_LevelData;

    protected override void Init()
    {
        m_LevelData = GameObject.FindWithTag(Tags.LevelIdentifier)?.GetComponent<LevelIdentifier>()?.levelData;
    }

    public void LoadNextLevelOrGoBackToMainMenu()
    {
        // If LevelManager is flagged DontDestroyOnLoad, it will be kept in next level (if any),
        // and it will be cleaner to clean the cached scene references first.
        // But we'll also need to set those again after loading the new scene.
        // Make sure to store current level index first.
        int currentLevelIndex = m_LevelData.levelIndex;
        m_LevelData = null;

        // first, do a brutal sync load
        if (currentLevelIndex < levelDataList.levelDataArray.Length - 1)
        {
            // we are not in the last level yet, proceed to next level
            int nextLevelIndex = currentLevelIndex + 1;
            LevelData nextLevelData = levelDataList.levelDataArray[nextLevelIndex];
            if (nextLevelData != null)
            {
                // convert scene enum to scene build index and load next level scene

                int nextLevelSceneBuildIndex = (int) nextLevelData.sceneEnum;

                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.AssertFormat(nextLevelSceneBuildIndex == nextLevelIndex + 1, nextLevelData,
                    "[LevelManager] FinishLevel: next level scene build index ({0}) is not next level index + 1 ({1}), " +
                    "where offset 1 represents the MainMenu scene. Did you add another non-level scene before " +
                    "the level scenes, causing ScenesEnum to offset all level scene build indices?",
                    nextLevelSceneBuildIndex, nextLevelIndex + 1);
                #endif

                SceneManager.LoadScene(nextLevelSceneBuildIndex);
                return;
            }

            Debug.LogErrorFormat(levelDataList, "[LevelManager] FinishLevel: missing level data for " +
                "next level at index {0} in levelDataList. Falling back to MainMenu scene.",
                nextLevelIndex);
        }

        // last level was finished, or we failed to find next level => clear current save and go back to title
        ExitToMainMenu();
    }

    public static void ExitToMainMenu()
    {
        SceneManager.LoadScene((int) ScenesEnum.MainMenu);
    }
}
