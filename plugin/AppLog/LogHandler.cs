using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

namespace ModSupport.AppLog {
	internal class LogHandler {
		private List<LogEntry> logEntries = new List<LogEntry>();
		private List<LogEntry> errors = new List<LogEntry>();
		private List<LogEntry> warnings = new List<LogEntry>();
		private volatile int numErrors;
		private volatile int numWarnings;

		public int NumErrors => numErrors;
		public int NumWarnings => numWarnings;
		
		public void HandleLog(string logString, string stackTrace, LogType type) {
			LogEntry logEntry = new LogEntry(logString, stackTrace, type);
			lock (logEntries) {
				logEntries.Add(logEntry);
			}

			switch (type) {
				case LogType.Error:
				case LogType.Exception:
					lock (errors) {
						errors.Add(logEntry);
					}
					Interlocked.Increment(ref numErrors);
					break;
				case LogType.Warning:
					lock (warnings) {
						warnings.Add(logEntry);
					}
					Interlocked.Increment(ref numWarnings);
					break;
			}
		}

		public string GenerateErrorReport() {
			StringBuilder sb = new StringBuilder();
			
			lock (errors) {
				foreach (LogEntry error in errors) {
					if (error.LogString != null) {
						sb.AppendLine(error.LogString);
					}
					if (error.StackTrace != null) {
						sb.AppendLine(error.StackTrace);
					}
				}
			}

			return sb.ToString();
		}
		
		public void CopyErrorsToClipboard() {
			GUIUtility.systemCopyBuffer = GenerateErrorReport();
		}
	}
}