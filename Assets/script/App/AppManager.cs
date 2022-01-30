using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsPattern;

public class AppManager : SingletonManager<AppManager>
{
    [SerializeField, Tooltip("Target framerate. -1 to keep platform default, using VSync if any.")]
    private int targetFrameRate = -1;

    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;

        // Flag don't destroy on load, so place the AppManager only once, in the first scene
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            ToggleFullScreen();
        }
    }

    public void ToggleFullScreen()
    {
        #if UNITY_EDITOR
        Debug.Log("[AppManager] Toggle fullscreen (ignored in Editor)");
        #else
        if (Screen.fullScreen)
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
        else
        {
            // Note that current resolution is the native resolution
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.ExclusiveFullScreen);
        }
        #endif
    }
}
