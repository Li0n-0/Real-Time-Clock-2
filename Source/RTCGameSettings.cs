using System;
using UnityEngine;

namespace RealTimeClock2
{
	public class RTCGameSettings : GameParameters.CustomParameterNode
	{
		public override string Title {
			get {
				return "Real Time Clock";
			}
		}

		public override string Section {
			get {
				return "Real Time Clock";
			}
		}

		public override string DisplaySection {
			get {
				return "Real Time Clock";
			}
		}

		public override int SectionOrder {
			get {
				return 1;
			}
		}

		public override GameParameters.GameMode GameMode {
			get {
				return GameParameters.GameMode.ANY;
			}
		}

		public override bool HasPresets {
			get {
				return false;
			}
		}

		[GameParameters.CustomParameterUI ("Use 24h format")]
		public bool use24 = true;

		[GameParameters.CustomParameterUI ("Enable in Space Center")]
		public bool enableKSC = true;

		[GameParameters.CustomParameterUI ("Enable in Editor")]
		public bool enableEditor = true;

		[GameParameters.CustomParameterUI ("Enable in Tracking Station")]
		public bool enableTS = true;

		[GameParameters.CustomParameterUI ("Enable in Flight")]
		public bool enableFlight = true;

		[GameParameters.CustomStringParameterUI ("\n\nFor the Mission Buileder use the setting file at GameData/RealTimeClock2/PluginData/Settings.cfg", autoPersistance = false, lines = 6)]
		public string dummy = "Enable by default";

		public override bool Enabled (System.Reflection.MemberInfo member, GameParameters parameters)
		{
			if (member.Name == "dummy") {
				if (Expansions.ExpansionsLoader.IsExpansionInstalled ("MakingHistory")) {
					return true;
				}
				return false;
			}
			return true;
		}
	}
}

