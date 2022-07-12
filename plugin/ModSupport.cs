using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ModSupport.AppLog;
using ModSupport.UI;
using UnityEngine;

namespace ModSupport {
	[BepInPlugin(GUID, NAME, VERSION)]
	public class ModSupport : BaseUnityPlugin {
		public const string GUID = "faeryn.modsupport";
		public const string NAME = "ModSupport";
		public const string VERSION = "1.0.0";
		internal static ManualLogSource Log;
		internal static readonly LogHandler LogHandler = new LogHandler();

		internal void Awake() {
			Application.logMessageReceivedThreaded += LogHandler.HandleLog;
			Log = this.Logger;
			Log.LogMessage($"Starting {NAME} {VERSION}");
			ModListMenu.InitializePrefab();
			new Harmony(GUID).PatchAll();
		}
		
	}
}