using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace ModSupport.AppLog {
	internal class LogHandler {
		private List<LogEntry> logEntries = new List<LogEntry>();
		private volatile int numErrors;
		private volatile int numExceptions;

		public List<LogEntry> GetCopyOfLogEntries() {
			List<LogEntry> copy;
			lock (logEntries) {
				copy = new List<LogEntry>(logEntries);
			}
			return copy;
		}

		public int NumErrors => numErrors;
		public int NumExceptions => numExceptions;
		
		public void HandleLog(string logString, string stackTrace, LogType type) {
			LogEntry logEntry = new LogEntry(DateTime.Now, type, logString, stackTrace);
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
	}
}