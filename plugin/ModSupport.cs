using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ModSupport.UI;

namespace ModSupport {
	[BepInPlugin(GUID, NAME, VERSION)]
	public class ModSupport : BaseUnityPlugin {
		public const string GUID = "faeryn.modsupport";
		public const string NAME = "ModSupport";
		public const string VERSION = "1.0.0";
		internal static ManualLogSource Log;

		internal void Awake() {
			Log = this.Logger;
			Log.LogMessage($"Starting {NAME} {VERSION}");
			UIHelper.Initialize();
			new Harmony(GUID).PatchAll();
		}
	}
}