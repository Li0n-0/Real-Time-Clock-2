using System;
using UnityEngine;

namespace RealTimeClock2
{
	[KSPAddon(KSPAddon.Startup.AllGameScenes, false)]
	public class RealTimeClock : MonoBehaviour
	{

		private Rect windowPos = new Rect(0, 0, 75f, 20f);
		private bool show24 = true;
		private string dateFormat;
		private GameScenes currentScene;
		private bool inSPH = false;
		private RealTimeSettings settings;
		private GUIStyle centeredStyle;
		private bool hideGUI = false;
		private bool draw = false;

		public void Start ()
		{
			settings = new RealTimeSettings ();
			show24 = settings.is24;
			currentScene = HighLogic.LoadedScene;

			// Fetch position
			switch (currentScene)
			{
			case GameScenes.SPACECENTER:
				windowPos.x = settings.KSCPosX;
				windowPos.y = settings.KSCPosY;
				draw = settings.inKSC;
				break;
			case GameScenes.EDITOR:
				if (EditorDriver.editorFacility == EditorFacility.SPH) {
					inSPH = true;
					windowPos.x = settings.SPHPosX;
					windowPos.y = settings.SPHPosY;
					draw = settings.inSPH;
				} else {
					windowPos.x = settings.VABPosX;
					windowPos.y = settings.VABPosY;
					draw = settings.inVAB;
				}
				break;
			case GameScenes.TRACKSTATION:
				windowPos.x = settings.trackStationPosX;
				windowPos.y = settings.trackStationPosY;
				draw = settings.inTrackStation;
				break;
			case GameScenes.FLIGHT:
				windowPos.x = settings.flightPosX;
				windowPos.y = settings.flightPosY;
				draw = settings.inFlight;
				break;
			}

			// Fetch 24h display
			if (show24) {
				dateFormat = "HH:mm";
			} else {
				dateFormat = "t";
			}

			GameEvents.onHideUI.Add (HideUI);
			GameEvents.onShowUI.Add (ShowUI);
			GameEvents.onGameSceneSwitchRequested.Add (SceneSwitch);
		}

		public void OnDestroy ()
		{
			GameEvents.onHideUI.Remove (HideUI);
			GameEvents.onShowUI.Remove (ShowUI);
			GameEvents.onGameSceneSwitchRequested.Remove (SceneSwitch);
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
			settings.Save (eData.from, windowPos, show24, inSPH);
		}

		private void OnGUI ()
		{
			// Get the current skin
			GUI.skin = HighLogic.Skin;

			centeredStyle = GUI.skin.GetStyle("Label");
			centeredStyle.alignment = TextAnchor.UpperCenter;
			if (settings.fontBold) {
				centeredStyle.fontStyle = FontStyle.Bold;
			}

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

			if (Event.current.type == EventType.mouseUp && Event.current.button == 1)
			{
				Toggle24 ();
			}

			GUI.Label (new Rect (5, 1, windowPos.width - 10, windowPos.height), curTime);

			GUI.DragWindow ();
		}

		private void Toggle24 ()
		{
			if (show24) {
				show24 = false;
				dateFormat = "t";
			} else {
				show24 = true;
				dateFormat = "HH:mm";
			}
		}
	}
}

