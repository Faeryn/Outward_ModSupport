using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

namespace ModSupport.AppLog {
	internal class LogHandler {
		private List<LogEntry> logEntries = new List<LogEntry>();
		private volatile int numErrors;
		private volatile int numExceptions;

		public int NumErrors => numErrors;
		public int NumExceptions => numExceptions;
		
		public void HandleLog(string logString, string stackTrace, LogType type) {
			LogEntry logEntry = new LogEntry(logString, stackTrace, type);
			lock (logEntries) {
				logEntries.Add(logEntry);
			}

			switch (type) {
				case LogType.Error:
					Interlocked.Increment(ref numErrors);
					break;
				case LogType.Exception:
					Interlocked.Increment(ref numExceptions);
					break;
			}
		}

		public string GenerateErrorReport() {
			StringBuilder sb = new StringBuilder();
			
			// TODO Once the model is finalized in the server project
			
			return sb.ToString();
		}
		
		public void CopyErrorsToClipboard() {
			GUIUtility.systemCopyBuffer = GenerateErrorReport();
		}
	}
}