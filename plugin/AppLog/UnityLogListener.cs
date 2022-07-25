using UnityEngine;

namespace ModSupport.AppLog {
	public class UnityLogListener {

		private const string UnitySource = "Unity";

		internal void Attach() {
			Application.logMessageReceivedThreaded += LogMessageReceived;
		}

		private void LogMessageReceived(string logMessage, string stackTrace, LogType type) {
			ModSupport.LogHandler.AppendLog(UnityToLogLevel(type), UnitySource, logMessage, stackTrace);
		}
		
		private LogLevel UnityToLogLevel(LogType logType) {
			switch (logType) {
				case LogType.Error:
					return LogLevel.Error;
				case LogType.Assert:
					return LogLevel.Debug;
				case LogType.Warning:
					return LogLevel.Warning;
				case LogType.Log:
					return LogLevel.Message;
				case LogType.Exception:
					return LogLevel.Fatal;
				default:
					return LogLevel.None;
			}
		}

	}
}