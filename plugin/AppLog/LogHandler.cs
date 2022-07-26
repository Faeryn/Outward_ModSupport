using System;
using System.Collections.Generic;
using System.Threading;

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
		
		public bool HasExceptions => NumExceptions > 0;
		
		public void AppendLog(LogLevel logLevel, string source, string logString, string stackTrace) {
			LogEntry logEntry = new LogEntry(DateTime.Now, source, logLevel, logString, stackTrace);
			lock (logEntries) {
				logEntries.Add(logEntry);
			}

			switch (logLevel) {
				case LogLevel.Error:
					Interlocked.Increment(ref numErrors);
					break;
				case LogLevel.Fatal:
					Interlocked.Increment(ref numExceptions);
					break;
			}
		}
	}
}