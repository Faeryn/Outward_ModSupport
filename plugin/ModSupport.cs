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
		public const string VERSION = "1.1.0";
		public const string CONFIG_BASIC = "1: Basic Configuration";
		public const string CONFIG_ADVANCED = "2: Advanced Configuration";
		internal static ManualLogSource Log;
		internal static ModSupport Instance;
		internal static readonly LogHandler LogHandler = new LogHandler();
		internal static readonly ReportManager ReportManager = new ReportManager();
		public readonly ModListManager ModListManager = new ModListManager();
		private readonly UnityLogListener unityLogListener = new UnityLogListener();
		private readonly BepInExLogListener bepInExLogListener = new BepInExLogListener();

		public static ConfigEntry<bool> OnlineEnabled;
		public static ConfigEntry<bool> ShowMsgBoxOnException;
		public static ConfigEntry<bool> SendOnExit;
		public static ConfigEntry<bool> SendOnLoadingExceptions;
		public static ConfigEntry<bool> SilentSend;
		public static ConfigEntry<bool> ErrorsAdvancedMode;
		public static ConfigEntry<bool> SendOnlyErrors;

		internal void Awake() {
			Instance = this;
			Log = this.Logger;
			Log.LogMessage($"Starting {NAME} {VERSION}");
			unityLogListener.Attach();
			bepInExLogListener.Attach();
			InitializeConfig();
			ModListMenu.InitializePrefab();
			new Harmony(GUID).PatchAll();
		}

		private void InitializeConfig() {
			OnlineEnabled = Config.Bind(CONFIG_BASIC, "Online features", true, "Enables online features, such as error reporting.");
			SendOnExit = Config.Bind(CONFIG_BASIC, "Show alert when exiting", true, "Shows an alert on exit if there are errors, with the option to send report.");
			SendOnLoadingExceptions = Config.Bind(CONFIG_BASIC, "Show alert on endless loading screen", true, "Shows an alert if too many errors happen during loading.");
			SendOnlyErrors = Config.Bind(CONFIG_BASIC, "Send only errors", false, "Sends only errors instead of the entire log. It's a good idea to use this if your bandwidth is limited.");
			
			SilentSend = Config.Bind(CONFIG_ADVANCED, "Send reports without asking", false, "Sends reports to the ModReport server without asking, and does not display any popups on response (success or failure).");
			ShowMsgBoxOnException = Config.Bind(CONFIG_ADVANCED, "Show alert on error", false, "Shows an alert every time an error happens, with the option to send report. This option is intended for debugging purposes, and you should not turn it on unless you want to be interrupted every time an error happens.");
			ErrorsAdvancedMode = Config.Bind(CONFIG_ADVANCED, "Errors advanced mode", false, "Shows the number of (actual) errors (instead of just exceptions). This feature is intended for developers and advanced users.");
		}
		
	}
}