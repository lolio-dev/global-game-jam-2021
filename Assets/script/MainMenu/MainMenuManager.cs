// hsandt: Most scripts in MainMenu folder are based on my own:
// https://github.com/hsandt/dragon-raid/tree/develop/Assets/Scripts/Menu
// (MIT License)

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityConstants;
using CommonsPattern;

/// Main Menu Manager
/// SEO: before all sub-menu scripts (for Instance access)
public class MainMenuManager : SingletonManager<MainMenuManager>
{
    [Header("Assets")]

    [Tooltip("Level Data List asset")]
    public LevelDataList levelDataList;


    [Header("Scene references")]

    [Tooltip("Canvas Main Menu")]
    public CanvasMainMenu canvasMainMenu;


    /* Cached scene references */

    private MainMenu m_MainMenu;


    /* State */

    private readonly Stack<Menu> m_MenuStack = new Stack<Menu>();


    protected override void Init()
    {
        base.Init();

        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.AssertFormat(levelDataList != null, this, "[MainMenuManager] Init: Level Data List not set on {0}", this);
        #endif

        // Retrieve Canvas Main Menu if not set
        if (canvasMainMenu == null)
        {
            GameObject canvasTitleMenuObject = GameObject.FindWithTag(Tags.CanvasMainMenu);
            if (canvasTitleMenuObject)
            {
                canvasMainMenu = canvasTitleMenuObject.GetComponent<CanvasMainMenu>();
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.AssertFormat(canvasMainMenu != null, this, "[MainMenuManager] Init: Canvas Main Menu script not found on {0}", canvasTitleMenuObject);
                #endif
            }
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            else
            {
                Debug.AssertFormat(canvasMainMenu != null, this, "[MainMenuManager] Init: No object found with tag CanvasMainMenu");
            }
            #endif
        }
    }

    private void Start()
    {
        // Note that we prefer hiding things in Start than in Awake,
        // because SEO guarantees this class is initialized before any sub-menu, so Awake would be too early,
        // while Start allows sub-menus to call their own Awake to setup things and assert on bad things early.
        // ! This means sub-menus should *not* rely on their Start to initialize things required before their Show.

        // Immediately hide whole main menu until we want to show it. Don't use Hide(), which may contain an animation
        // (sub-menus will be hidden below anything)
        canvasMainMenu.gameObject.SetActive(false);

        // Hide all menus and remember which was the main menu (for ShowMainMenu later)
        // Of course the whole tree is hidden for now, but it will allow us not to have to hide all irrelevant sub-menus
        // later in ShowMainMenu
        Transform menusParent = canvasMainMenu.menusParent;
        if (menusParent)
        {
            var menus = menusParent.GetComponentsInChildren<Menu>();
            foreach (Menu menu in menus)
            {
                var mainMenu = menu as MainMenu;
                if (mainMenu != null)
                {
                    m_MainMenu = mainMenu;
                    Debug.LogFormat("Found main menu: {0}", m_MainMenu);
                }

                // Do not call Hide() which may contain some animation, immediately deactivate instead
                menu.gameObject.SetActive(false);
            }

            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.AssertFormat(m_MainMenu != null, menusParent, "No Main Menu component found under {0}", menusParent);
            #endif
        }
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        else
        {
            Debug.LogErrorFormat(canvasMainMenu, "No Menu parent set on {0}", canvasMainMenu);
        }
        #endif

        ShowMainMenu();
    }

    /// Show canvas and enter main menu
    /// Should be called once on Start, when ready
    private void ShowMainMenu()
    {
        canvasMainMenu.Show();
        EnterMenu(m_MainMenu);
    }

    public void EnterMenu(Menu menu)
    {
        if (menu == null)
        {
            throw new ArgumentNullException(nameof(menu));
        }

        // Hide current menu, if any
        if (m_MenuStack.Count > 0)
        {
            m_MenuStack.Peek().Hide();
        }

        // Push and show next menu
        m_MenuStack.Push(menu);
        menu.Show();

        UpdateTitleVisibility(menu);
    }

    public void GoBackToPreviousMenu()
    {
        // Pop and hide current menu
        Menu menu = m_MenuStack.Pop();
        menu.Hide();

        // Show previous menu, if any
        if (m_MenuStack.Count > 0)
        {
            Menu previousMenu = m_MenuStack.Peek();
            previousMenu.Show();
            UpdateTitleVisibility(previousMenu);
        }
    }

    private void UpdateTitleVisibility(Menu lastMenu)
    {
        if (lastMenu.ShouldShowTitle())
        {
            // if title is already shown, does nothing
            canvasMainMenu.ShowTitle();
        }
        else
        {
            // if title is already hidden, does nothing
            canvasMainMenu.HideTitle();
        }
    }

    /// Start level with given levelIndex
    public void StartLevel(int levelIndex)
    {
        if (levelDataList.levelDataArray.Length > levelIndex)
        {
            LevelData levelData = levelDataList.levelDataArray[levelIndex];
            if (levelData != null)
            {
                SceneManager.LoadScene((int)levelData.sceneEnum);
            }
            else
            {
                Debug.LogErrorFormat(levelDataList, "[MainMenuManager] StartGame: Level Data List entry " +
                    "for levelIndex {0} is null", levelIndex);
            }
        }
        else
        {
            Debug.LogErrorFormat(levelDataList, "[MainMenuManager] StartGame: Level Data List has only {0} entries, " +
                "cannot get entry for levelIndex {1}",
                levelDataList.levelDataArray.Length, levelIndex);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Cancel();
        }
    }

    private void Cancel()
    {
        if (m_MenuStack.Count > 0)
        {
            Menu menu = m_MenuStack.Peek();
            if (menu.CanGoBack())
            {
                // For now, works in all cases, but when we add special behaviour on Back
                // like prompting for changes or auto-applying changes in Options,
                // we may want to call some overridden OnBack method on the current menu
                GoBackToPreviousMenu();
            }
        }
    }
}
