using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using ModSupport.AppLog;
using ModSupport.ModSource;
using ModSupport.Report;
using ModSupport.UI;
using UnityLogListener = ModSupport.AppLog.UnityLogListener;

namespace ModSupport {
	[BepInPlugin(GUID, NAME, VERSION)]
	public class ModSupport : BaseUnityPlugin {
		public const string GUID = "faeryn.modsupport";
		public const string NAME = "ModSupport";
		public const string VERSION = "1.0.0";
		public const string DISPLAY_NAME = "Mod Support";
		internal static ManualLogSource Log;
		internal static ModSupport Instance;
		internal static readonly LogHandler LogHandler = new LogHandler();
		internal static readonly ReportManager ReportManager = new ReportManager();
		public readonly ModListManager ModListManager = new ModListManager();
		private readonly UnityLogListener unityLogListener = new UnityLogListener();
		private readonly BepInExLogListener bepInExLogListener = new BepInExLogListener();

		public static ConfigEntry<bool> ShowMsgBoxOnException;
		public static ConfigEntry<bool> ShowMsgBoxOnExceptionExit;

		internal void Awake() {
			Instance = this;
			unityLogListener.Attach();
			bepInExLogListener.Attach();
			ModListManager.Initialize();
			Log = this.Logger;
			Log.LogMessage($"Starting {NAME} {VERSION}");
			InitializeConfig();
			ModListMenu.InitializePrefab();
			new Harmony(GUID).PatchAll();
		}
		
		private void InitializeConfig() {
			ShowMsgBoxOnException = Config.Bind(DISPLAY_NAME, "Show alert on error", false, "Shows an alert every time an error happens, with the option to send report");
			ShowMsgBoxOnExceptionExit = Config.Bind(DISPLAY_NAME, "Show alert on error when exiting", true, "Shows an alert on exit if there are errors, with the option to send report");
		}
		
	}
}