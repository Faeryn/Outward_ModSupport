using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ModSupport.AppLog;
using ModSupport.Report;
using ModSupport.UI;
using UnityLogListener = ModSupport.AppLog.UnityLogListener;

namespace ModSupport {
	[BepInPlugin(GUID, NAME, VERSION)]
	public class ModSupport : BaseUnityPlugin {
		public const string GUID = "faeryn.modsupport";
		public const string NAME = "ModSupport";
		public const string VERSION = "1.0.0";
		internal static ManualLogSource Log;
		internal static ModSupport Instance;
		internal static readonly LogHandler LogHandler = new LogHandler();
		internal static readonly ReportManager ReportManager = new ReportManager();
		private readonly UnityLogListener unityLogListener = new UnityLogListener();
		private readonly BepInExLogListener bepInExLogListener = new BepInExLogListener();

		internal void Awake() {
			Instance = this;
			unityLogListener.Attach();
			bepInExLogListener.Attach();
			Log = this.Logger;
			Log.LogMessage($"Starting {NAME} {VERSION}");
			ModListMenu.InitializePrefab();
			new Harmony(GUID).PatchAll();
		}
		
	}
}