using UnityEngine;

namespace ModSupport.AppLog {
	public class LogEntry {
		public string LogString { get; }
		public string StackTrace { get; }
		public LogType LogType { get; }

		public LogEntry(string logString, string stackTrace, LogType logType) {
			LogString = logString;
			StackTrace = stackTrace;
			LogType = logType;
		}
	}
}