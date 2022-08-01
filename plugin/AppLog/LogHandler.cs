using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ModSupport.AppLog {
	internal class LogHandler {
		private static readonly TimeSpan FindLastMatchingEntryMaxTimeSpan = TimeSpan.FromMinutes(5); 
		
		private List<LogProcessor> logProcessors = new List<LogProcessor> {
			new RemoveSteamID()
		};
		private List<LogEntry> logEntries = new List<LogEntry>();
		private volatile int numErrors;
		private volatile int numExceptions;

		public IEnumerable<LogEntry> GetCopyOfLogEntries() {
			List<LogEntry> copy;
			lock (logEntries) {
				copy = new List<LogEntry>(logEntries);
			}
			return copy;
		}
		
		public IEnumerable<LogEntry> GetCopyOfErrors() {
			List<LogEntry> copy;
			lock (logEntries) {
				copy = logEntries.Where(logEntry => logEntry.LogLevel == LogLevel.Fatal).ToList();
			}
			return copy;
		}

		public int NumErrors => numErrors;
		public int NumExceptions => numExceptions;
		
		public bool HasExceptions => NumExceptions > 0;


		private (int, LogEntry) FindLastMatchingLogEntry(LogLevel logLevel, string source, string logString, string stackTrace) {
			if (logEntries.Count == 0) {
				return (-1, null);
			}

			DateTime timeThreshold = DateTime.Now - FindLastMatchingEntryMaxTimeSpan;
			for (int index = logEntries.Count - 1; index > 0; index--) {
				LogEntry entry = logEntries[index];
				if (entry.LogTime < timeThreshold) {
					return (-1, null);
				}
				if (entry.ContentEquals(source, logLevel, logString, stackTrace)) {
					return (index, entry);
				}
			}
			return (-1, null);
		}
		
		public void AppendLog(LogLevel logLevel, string source, string logString, string stackTrace) {
			logString = logProcessors.Aggregate(logString, (current, logProcessor) => logProcessor.ProcessLog(current));
			lock (logEntries) {
				(int, LogEntry) logEntry = FindLastMatchingLogEntry(logLevel, source, logString, stackTrace);
				if (logEntry.Item1 != -1) {
					logEntries[logEntry.Item1] = logEntry.Item2.WithIncrementedCount();
				} else {
					logEntries.Add(new LogEntry(DateTime.Now, source, logLevel, logString, stackTrace));
				}
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