// hsandt: copied from my own script at
// https://bitbucket.org/hsandt/unity-commons-editor/src/develop/EditorScreenshot.cs

﻿// Created by Long Nguyen Huu
// 2016.05.15
// MIT License

using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace CommonsEditor
{

	public class EditorScreenshot : EditorWindow
	{
		const string defaultScreenshotFolderPath = "Screenshots";
		const string defaultScreenshotFilenamePrefix = "screenshot_";
		
		// EXPERIMENTAL: default parameters for hi-res screenshot
		const int defaultHiResWidth = 3840;
		const int defaultHiResHeight = 2160;

		string screenshotFolderPath = defaultScreenshotFolderPath;
		string screenshotFilenamePrefix = defaultScreenshotFilenamePrefix;
		int nextScreenshotIndex = 0;
		
		// EXPERIMENTAL: parameters for hi-res screenshot
		public int hiResWidth = defaultHiResWidth;
		public int hiResHeight = defaultHiResHeight;

		[MenuItem("Window/Editor Screenshot")]
		static void Init()
		{
			GetOrCreateWindow();
		}

		[MenuItem("Tools/Take Screenshot _F11")]
		static void StaticTakeScreenshot()
		{
			GetOrCreateWindow().TakeScreenshot();
		}

		static EditorScreenshot GetOrCreateWindow()
		{
			EditorScreenshot editorScreenshot = GetWindow<EditorScreenshot>(title: "Screenshot");

			if (EditorPrefs.HasKey($"EditorScreenshot.{Application.productName}.screenshotFolderPath"))
			{
				editorScreenshot.screenshotFolderPath = EditorPrefs.GetString($"EditorScreenshot.{Application.productName}.screenshotFolderPath");
			}
			
			// if empty, revert to default
			if (string.IsNullOrWhiteSpace(editorScreenshot.screenshotFolderPath))
			{
				editorScreenshot.screenshotFolderPath = defaultScreenshotFolderPath;
				EditorPrefs.SetString($"EditorScreenshot.{Application.productName}.screenshotFolderPath", defaultScreenshotFolderPath);
			}

			if (EditorPrefs.HasKey($"EditorScreenshot.{Application.productName}.screenshotFilenamePrefix"))
			{
				editorScreenshot.screenshotFilenamePrefix = EditorPrefs.GetString($"EditorScreenshot.{Application.productName}.screenshotFilenamePrefix");
			}
			
			// if empty, revert to default
			if (string.IsNullOrWhiteSpace(editorScreenshot.screenshotFilenamePrefix))
			{
				editorScreenshot.screenshotFilenamePrefix = defaultScreenshotFilenamePrefix;
				EditorPrefs.SetString($"EditorScreenshot.{Application.productName}.screenshotFilenamePrefix", defaultScreenshotFilenamePrefix);
			}

			if (EditorPrefs.HasKey($"EditorScreenshot.{Application.productName}.nextScreenshotIndex"))
			{
				editorScreenshot.nextScreenshotIndex = EditorPrefs.GetInt($"EditorScreenshot.{Application.productName}.nextScreenshotIndex");
			}

			if (EditorPrefs.HasKey($"EditorScreenshot.{Application.productName}.hiResWidth"))
			{
				editorScreenshot.hiResWidth = EditorPrefs.GetInt($"EditorScreenshot.{Application.productName}.hiResWidth");
			}

			if (EditorPrefs.HasKey($"EditorScreenshot.{Application.productName}.hiResHeight"))
			{
				editorScreenshot.hiResHeight = EditorPrefs.GetInt($"EditorScreenshot.{Application.productName}.hiResHeight");
			}
			
			// if one dimension is 0, revert to default
			if (editorScreenshot.hiResWidth == 0 || editorScreenshot.hiResHeight == 0)
			{
				editorScreenshot.hiResWidth = defaultHiResWidth;
				editorScreenshot.hiResHeight = defaultHiResHeight;
				EditorPrefs.SetInt($"EditorScreenshot.{Application.productName}.hiResWidth", defaultHiResWidth);
				EditorPrefs.SetInt($"EditorScreenshot.{Application.productName}.hiResHeight", defaultHiResHeight);

			}

			return editorScreenshot;
		}

		void OnGUI()
		{
			EditorGUI.BeginChangeCheck();

			GUIContent savePathLabel = new GUIContent("Save path", "Save path of the screenshots, relative from the project root");
			screenshotFolderPath = EditorGUILayout.TextField(savePathLabel, screenshotFolderPath);
			screenshotFilenamePrefix = EditorGUILayout.TextField("Screenshot prefix", screenshotFilenamePrefix);
			nextScreenshotIndex = EditorGUILayout.IntField("Next screenshot index", nextScreenshotIndex);
			hiResWidth = EditorGUILayout.IntField("Hi-res width", hiResWidth);
			hiResHeight = EditorGUILayout.IntField("Hi-res height", hiResHeight);

			if (EditorGUI.EndChangeCheck()) {
				EditorPrefs.SetString($"EditorScreenshot.{Application.productName}.screenshotFolderPath", screenshotFolderPath);
				EditorPrefs.SetString($"EditorScreenshot.{Application.productName}.screenshotFilenamePrefix", screenshotFilenamePrefix);
				EditorPrefs.SetInt($"EditorScreenshot.{Application.productName}.nextScreenshotIndex", nextScreenshotIndex);
				EditorPrefs.SetInt($"EditorScreenshot.{Application.productName}.hiResWidth", hiResWidth);
				EditorPrefs.SetInt($"EditorScreenshot.{Application.productName}.hiResHeight", hiResHeight);
			}

			if (GUILayout.Button("Take screenshot")) TakeScreenshot();
			if (GUILayout.Button("Take hi-res screenshot")) TakeHiresScreenshot();
		}

		void TakeScreenshot()
		{
			if (string.IsNullOrWhiteSpace(screenshotFolderPath))
			{
				Debug.LogWarning("Screenshot Folder Path is empty, cannot take screenshot.");
				return;
			}
			
			// get name of current focused window, which should be "  (UnityEditor.GameView)" if it is a Game view
			string focusedWindowName = EditorWindow.focusedWindow.ToString();
			if (!focusedWindowName.Contains("UnityEditor.GameView")) {
				// since no Game view is focused right now, focus on any Game view, or create one if needed
				Type gameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
				EditorWindow.GetWindow(gameViewType);
			}

			// Tried getting the last focused window, but does not always work (even for focused window!)
			// Type gameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
			// EditorWindow lastFocusedGameView = (EditorWindow) gameViewType.GetField("s_LastFocusedGameView", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
			// if (lastFocusedGameView != null) {
			// 	lastFocusedGameView.Focus();
			// } else {
			// 	// no Game view created since editor launch, create one
			// 	Type gameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
			// 	EditorWindow.GetWindow(gameViewType);
			// }

			try
			{
				if (!Directory.Exists(screenshotFolderPath))
				{
					Directory.CreateDirectory(screenshotFolderPath);
				}
				string path = string.Format("{0}/{1}{2:00}.png", screenshotFolderPath, screenshotFilenamePrefix, nextScreenshotIndex);
				ScreenCapture.CaptureScreenshot(path);

				Debug.LogFormat("Screenshot recorded at {0} ({1})", path, UnityStats.screenRes);

				IncrementScreenshotIndex();
			}
			catch (IOException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		// EXPERIMENTAL: hi-res screenshot
		// http://answers.unity3d.com/questions/22954/how-to-save-a-picture-take-screenshot-from-a-camer.html
		// For transparency, insert code from http://answers.unity3d.com/questions/12070/capture-rendered-scene-to-png-with-background-tran.html
		void TakeHiresScreenshot ()
		{
			Camera camera = Camera.main;
			if (camera == null) {
				Debug.LogWarning("No main camera found");
				return;
			}

			RenderTexture rt = new RenderTexture(hiResWidth, hiResHeight, 24);
			camera.targetTexture = rt;
			Texture2D screenShot = new Texture2D(hiResWidth, hiResHeight, TextureFormat.RGB24, false);
			camera.Render();
			RenderTexture.active = rt;
			screenShot.ReadPixels(new Rect(0, 0, hiResWidth, hiResHeight), 0, 0);
			camera.targetTexture = null;
			RenderTexture.active = null; // JC: added to avoid errors
			if (EditorApplication.isPlaying)  // ADDED
				Destroy(rt);
			else
				DestroyImmediate(rt);
			byte[] bytes = screenShot.EncodeToPNG();

			try
			{
				if (!Directory.Exists(screenshotFolderPath))
				{
					Directory.CreateDirectory(screenshotFolderPath);
				}
				string path = string.Format("{0}/{1}{2:00} (hires).png", screenshotFolderPath, screenshotFilenamePrefix, nextScreenshotIndex);
				File.WriteAllBytes(path, bytes);

				Debug.LogFormat("Hires Screenshot recorded at {0} ({1})", path, UnityStats.screenRes);

				IncrementScreenshotIndex();
			}
			catch (IOException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private void IncrementScreenshotIndex()
		{
			++nextScreenshotIndex;
			EditorPrefs.SetInt($"EditorScreenshot.{Application.productName}.nextScreenshotIndex", nextScreenshotIndex);
		}
	}

}
