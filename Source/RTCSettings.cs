using System;
using UnityEngine;

namespace RealTimeClock2
{
	public class RTCSettings
	{
		private static ConfigNode nodeSettings;

		private static ConfigNode nodeMission;
		private static ConfigNode nodePosition;

		// Making Builder
		public static bool enableInMB = true;
		public static bool use24InMB = true;

		// Position
		public static Vector2 posKSC = new Vector2 (Screen.width / 4, 0);
		public static Vector2 posEditor = new Vector2 (Screen.width / 4, 0);
		public static Vector2 posTS = new Vector2 (Screen.width / 4, 0);
		public static Vector2 posFlight = new Vector2 (Screen.width / 4, 0);
		public static Vector2 posMB = new Vector2 (Screen.width / 4, 0);

		static RTCSettings ()
		{
			Load ();
			WriteSave ();
		}

		public static void Load ()
		{
			// Check for the settings file
			nodeSettings = ConfigNode.Load (KSPUtil.ApplicationRootPath + "GameData/RealTimeClock2/PluginData/Settings.cfg");
			if (nodeSettings == null) {
				nodeSettings = new ConfigNode ();
			}

			// Check for nodes in settings file
			if (! nodeSettings.HasNode("Mission_Builder")) {
				nodeSettings.AddNode ("Mission_Builder");
			}
			nodeMission = nodeSettings.GetNode ("Mission_Builder");

			if (! nodeSettings.HasNode("Windows_Position")) {
				nodeSettings.AddNode ("Windows_Position");
			}
			nodePosition = nodeSettings.GetNode ("Windows_Position");

			// Check for value in nodes
			// Mission Builder
			if (nodeMission.HasValue ("enable_in_mission_builder")) {
				enableInMB = bool.Parse (nodeMission.GetValue ("enable_in_mission_builder"));
			}
			nodeMission.SetValue ("enable_in_mission_builder", enableInMB, true);

			if (nodeMission.HasValue ("use_24h_format_in_mission_builder")) {
				use24InMB = bool.Parse (nodeMission.GetValue ("use_24h_format_in_mission_builder"));
			}
			nodeMission.SetValue ("use_24h_format_in_mission_builder", use24InMB, true);

			// Window Position
			if (nodePosition.HasValue ("KSC_pos")) {
				posKSC = ConfigNode.ParseVector2 (nodePosition.GetValue ("KSC_pos"));
			}
			nodePosition.SetValue ("KSC_pos", posKSC, true);

			if (nodePosition.HasValue ("Editor_pos")) {
				posEditor = ConfigNode.ParseVector2 (nodePosition.GetValue ("Editor_pos"));
			}
			nodePosition.SetValue ("Editor_pos", posEditor, true);

			if (nodePosition.HasValue ("TS_pos")) {
				posTS = ConfigNode.ParseVector2 (nodePosition.GetValue ("TS_pos"));
			}
			nodePosition.SetValue ("TS_pos", posTS, true);

			if (nodePosition.HasValue ("Flight_pos")) {
				posFlight = ConfigNode.ParseVector2 (nodePosition.GetValue ("Flight_pos"));
			}
			nodePosition.SetValue ("Flight_pos", posFlight, true);

			if (nodePosition.HasValue ("MB_pos")) {
				posMB = ConfigNode.ParseVector2 (nodePosition.GetValue ("MB_pos"));
			}
			nodePosition.SetValue ("MB_pos", posMB, true);
		}

		public static void SavePos (string windowName, Vector2 pos)
		{
			nodePosition.SetValue (windowName, pos, true);
		}

		public static void WriteSave ()
		{
			nodeSettings.Save (KSPUtil.ApplicationRootPath + "GameData/RealTimeClock2/PluginData/Settings.cfg");
			Load ();
		}
	}
}

