using BepInEx.Logging;

namespace ModSupport.AppLog {
	public class BepInExLogListener : ILogListener {
		private const string BepinexUnitySource = "Unity Log"; // BepInEx.Unity.Logging.UnityLogSource SourceName

		internal void Attach() {
			Logger.Listeners.Add(this);
		}
		
		public void LogEvent(object sender, LogEventArgs eventArgs) {
			if (eventArgs.Source.SourceName == BepinexUnitySource) {
				return;
			}
			ModSupport.LogHandler.AppendLog(BepInExToLogLevel(eventArgs.Level), eventArgs.Source.SourceName, eventArgs.Data.ToString(), null);
		}

		private LogLevel BepInExToLogLevel(BepInEx.Logging.LogLevel level) {
			switch (level) {
				case BepInEx.Logging.LogLevel.None:
					return LogLevel.None;
				case BepInEx.Logging.LogLevel.Fatal:
					return LogLevel.Fatal;
				case BepInEx.Logging.LogLevel.Error:
					return LogLevel.Error;
				case BepInEx.Logging.LogLevel.Warning:
					return LogLevel.Warning;
				case BepInEx.Logging.LogLevel.Debug:
					return LogLevel.Debug;
				case BepInEx.Logging.LogLevel.Info:
					return LogLevel.Info;
				case BepInEx.Logging.LogLevel.Message:
					return LogLevel.Message;
				case BepInEx.Logging.LogLevel.All:
					return LogLevel.None;
				default:
					return LogLevel.None;
			}
		}
		
		public void Dispose() {
		}
	}
}