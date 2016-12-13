using System;
using UnityEngine;

namespace RealTimeClock2
{
	public class RealTimeSettings
	{
		public bool is24 = true;
		public bool fontBold = true;
		public bool inKSC = true;
		public float KSCPosX = 0f;
		public float KSCPosY = 0f;
		public bool inVAB = true;
		public float VABPosX = 0f;
		public float VABPosY = 0f;
		public bool inSPH = true;
		public float SPHPosX = 0f;
		public float SPHPosY = 0f;
		public bool inTrackStation = true;
		public float trackStationPosX = 0f;
		public float trackStationPosY = 0f;
		public bool inFlight = true;
		public float flightPosX = 0f;
		public float flightPosY = 0f;

		private string path = KSPUtil.ApplicationRootPath + "GameData/RealTimeClock2/PluginData/Settings.cfg";
		private ConfigNode settingsNode;
		private ConfigNode generalNode;
		private ConfigNode boolNode;
		private ConfigNode rectNode;

		public RealTimeSettings ()
		{
			int fourthOfScreen = Screen.width / 4;
			KSCPosX = fourthOfScreen;
			VABPosX = fourthOfScreen;
			SPHPosX = fourthOfScreen;
			trackStationPosX = fourthOfScreen;
			flightPosX = fourthOfScreen;

			if (Load() == false) { SaveToFile (); }
		}

		private bool Load ()
		{
			settingsNode = ConfigNode.Load (path);
			if (settingsNode == null) { return false; }

			if (settingsNode.HasNode("General")) {
				generalNode = settingsNode.GetNode ("General");
			} else { return false; }
			if (settingsNode.HasNode("Scene_Toggle")) {
				boolNode = settingsNode.GetNode ("Scene_Toggle");
			} else { return false; }
			if (settingsNode.HasNode("Position")) {
				rectNode = settingsNode.GetNode ("Position");
			} else { return false; }

			string[] paramGeneralNode = new string[] {
				"show_24",
				"bold_font"
			};
			string[] paramBoolNode = new string[] {
				"in_KSC",
				"in_VAB",
				"in_SPH",
				"in_tracking_station",
				"in_flight"
			};
			string[] paramRectNode = new string[] {
				"KSC_x",
				"KSC_y",
				"VAB_x",
				"VAB_y",
				"SPH_x",
				"SPH_y",
				"tracking_station_x",
				"tracking_station_y",
				"flight_x",
				"flight_y"
			};

			if (generalNode.HasValues(paramGeneralNode)) {
				is24 = bool.Parse (generalNode.GetValue ("show_24"));
				fontBold = bool.Parse (generalNode.GetValue ("bold_font"));
			} else { return false; }
			if (boolNode.HasValues(paramBoolNode)) {
				inKSC = bool.Parse (boolNode.GetValue ("in_KSC"));
				inVAB = bool.Parse (boolNode.GetValue ("in_VAB"));
				inSPH = bool.Parse (boolNode.GetValue ("in_SPH"));
				inTrackStation = bool.Parse (boolNode.GetValue ("in_tracking_station"));
				inFlight = bool.Parse (boolNode.GetValue ("in_flight"));
			} else { return false; }
			if (rectNode.HasValues(paramRectNode)) {
				KSCPosX = float.Parse (rectNode.GetValue ("KSC_x"));
				KSCPosY = float.Parse (rectNode.GetValue("KSC_y"));
				VABPosX = float.Parse (rectNode.GetValue("VAB_x"));
				VABPosY = float.Parse (rectNode.GetValue("VAB_y"));
				SPHPosX = float.Parse (rectNode.GetValue("SPH_x"));
				SPHPosY = float.Parse (rectNode.GetValue("SPH_y"));
				trackStationPosX = float.Parse (rectNode.GetValue("tracking_station_x"));
				trackStationPosY = float.Parse (rectNode.GetValue("tracking_station_y"));
				flightPosX = float.Parse (rectNode.GetValue("flight_x"));
				flightPosY = float.Parse (rectNode.GetValue("flight_y"));
			} else { return false; }

			return true;
		}

		private void SaveToFile ()
		{
			settingsNode = new ConfigNode ();
			settingsNode.AddNode ("General");
			settingsNode.AddNode ("Scene_Toggle");
			settingsNode.AddNode ("Position");
			generalNode = settingsNode.GetNode ("General");
			boolNode = settingsNode.GetNode ("Scene_Toggle");
			rectNode = settingsNode.GetNode ("Position");

			generalNode.AddValue ("show_24", is24);
			generalNode.AddValue ("bold_font", fontBold);

			boolNode.AddValue ("in_KSC", inKSC);
			boolNode.AddValue ("in_VAB", inVAB);
			boolNode.AddValue ("in_SPH", inSPH);
			boolNode.AddValue ("in_tracking_station", inTrackStation);
			boolNode.AddValue ("in_flight", inFlight);

			rectNode.AddValue ("KSC_x", KSCPosX);
			rectNode.AddValue ("KSC_y", KSCPosY);
			rectNode.AddValue ("VAB_x", VABPosX);
			rectNode.AddValue ("VAB_y", VABPosY);
			rectNode.AddValue ("SPH_x", SPHPosX);
			rectNode.AddValue ("SPH_y", SPHPosY);
			rectNode.AddValue ("tracking_station_x", trackStationPosX);
			rectNode.AddValue ("tracking_station_y", trackStationPosY);
			rectNode.AddValue ("flight_x", flightPosX);
			rectNode.AddValue ("flight_y", flightPosY);

			settingsNode.Save (path);
		}

		public void Save (GameScenes scene, Rect rect, bool show24, bool isSPH = false)
		{
			switch (scene) {
			case GameScenes.SPACECENTER:
				KSCPosX = rect.x;
				KSCPosY = rect.y;
				break;
			case GameScenes.EDITOR:
				if (isSPH) {
					SPHPosX = rect.x;
					SPHPosY = rect.y;
				} else {
					VABPosX = rect.x;
					VABPosY = rect.y;
				}
				break;
			case GameScenes.TRACKSTATION:
				trackStationPosX = rect.x;
				trackStationPosY = rect.y;
				break;
			case GameScenes.FLIGHT:
				flightPosX = rect.x;
				flightPosY = rect.y;
				break;
			}

			is24 = show24;
			this.SaveToFile ();
		}
	}
}

