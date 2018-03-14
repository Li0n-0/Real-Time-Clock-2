using System;
using UnityEngine;

namespace RealTimeClock2
{
	[KSPAddon(KSPAddon.Startup.EveryScene, false)]
	public class RealTimeClock : MonoBehaviour
	{

		private Rect windowPos = new Rect(0, 0, 75f, 20f);
		private string dateFormat;
		private GameScenes currentScene;
		private RTCGameSettings gameSettings;

		private GUIStyle centeredStyle;
		private bool hideGUI = false;
		private bool draw = false;

		public void Start ()
		{
			currentScene = HighLogic.LoadedScene;
			if (currentScene == GameScenes.CREDITS || currentScene == GameScenes.LOADING 
				|| currentScene == GameScenes.LOADINGBUFFER || currentScene == GameScenes.MAINMENU 
				|| currentScene == GameScenes.PSYSTEM || currentScene == GameScenes.SETTINGS)
			{
				Destroy (this);
				return;
			}

			if (currentScene == GameScenes.MISSIONBUILDER) {
				windowPos.position = RTCSettings.posMB;
				draw = RTCSettings.enableInMB;
				if (RTCSettings.use24InMB) {
					dateFormat = "HH:mm";
				} else {
					dateFormat = "t";
				}
			} else {
				gameSettings = HighLogic.CurrentGame.Parameters.CustomParams<RTCGameSettings> ();
				// Fetch 24h display
				SetFormat ();

				switch (currentScene)
				{
				case GameScenes.SPACECENTER:
					windowPos.position = RTCSettings.posKSC;
					draw = gameSettings.enableKSC;
					break;
				case GameScenes.EDITOR:
					windowPos.position = RTCSettings.posEditor;
					draw = gameSettings.enableEditor;
					break;
				case GameScenes.TRACKSTATION:
					windowPos.position = RTCSettings.posTS;
					draw = gameSettings.enableTS;
					break;
				case GameScenes.FLIGHT:
					windowPos.position = RTCSettings.posFlight;
					draw = gameSettings.enableFlight;
					break;
				default:
					Destroy (this);
					return;
				}
			}

			GameEvents.onHideUI.Add (HideUI);
			GameEvents.onShowUI.Add (ShowUI);
			GameEvents.onGameSceneSwitchRequested.Add (SceneSwitch);
			GameEvents.OnGameSettingsApplied.Add (SetFormat);
		}

		public void OnDestroy ()
		{
			GameEvents.onHideUI.Remove (HideUI);
			GameEvents.onShowUI.Remove (ShowUI);
			GameEvents.onGameSceneSwitchRequested.Remove (SceneSwitch);
			GameEvents.OnGameSettingsApplied.Remove (SetFormat);
		}

		private void SetFormat ()
		{
			if (gameSettings.use24) {
				dateFormat = "HH:mm";
			} else {
				dateFormat = "t";
			}
		}

		private void HideUI ()
		{
			hideGUI = true;
		}

		private void ShowUI ()
		{
			hideGUI = false;
		}

		private void SceneSwitch (GameEvents.FromToAction<GameScenes, GameScenes> eData)
		{
			string sceneName = "";

			switch (eData.from) {
			case GameScenes.SPACECENTER:
				sceneName = "KSC_pos";
				break;
			case GameScenes.EDITOR:
				sceneName = "Editor_pos";
				break;
			case GameScenes.TRACKSTATION:
				sceneName = "TS_pos";
				break;
			case GameScenes.FLIGHT:
				sceneName = "Flight_pos";
				break;
			case GameScenes.MISSIONBUILDER:
				sceneName = "MB_pos";
				break;
			}

			RTCSettings.SavePos (sceneName, windowPos.position);
			RTCSettings.WriteSave ();
		}

		private void OnGUI ()
		{
			// Get the current skin
			GUI.skin = HighLogic.Skin;

			centeredStyle = GUI.skin.GetStyle("Label");
			centeredStyle.alignment = TextAnchor.UpperCenter;
			centeredStyle.fontStyle = FontStyle.Bold;

			// Make sure the window isn't dragged off screen
			if (windowPos.x < 0) { windowPos.x = 0; }
			if (windowPos.y < 0) { windowPos.y = 0; }
			if (windowPos.x > Screen.width - windowPos.width) {
				windowPos.x = Screen.width - windowPos.width;
			}
			if (windowPos.y > Screen.height - windowPos.height) {
				windowPos.y = Screen.height - windowPos.height;
			}

			if (! hideGUI && draw) {
				windowPos = GUI.Window (641286, windowPos, DrawTime, "");
			}
		}

		private void DrawTime (int windowID)
		{
			string curTime = DateTime.Now.ToString (dateFormat);

			GUI.Label (new Rect (5, 1, windowPos.width - 10, windowPos.height), curTime);

			GUI.DragWindow ();
		}
	}
}

