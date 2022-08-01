using System;
using System.Runtime.Serialization;

namespace ModSupport.AppLog {
	[DataContract]
	public class LogEntry {
		[DataMember(Name="logTime")]
		public string LogTimeString {
			get => logTime.ToString("o", System.Globalization.CultureInfo.InvariantCulture);
			set => logTime = DateTime.Parse(value);
		}
		private DateTime logTime;
		
		[DataMember(Name="logLevel")]
		public string LogLevelString {
			get => logLevel.ToString();
			set => Enum.TryParse(value, out logLevel);
		}

		private LogLevel logLevel;
		
		[DataMember]
		private string logSource;
		
		[DataMember]
		private string text;
		
		[DataMember]
		private string stackTrace;

		[DataMember]
		private int count;

		public DateTime LogTime => logTime;

		public LogLevel LogLevel => logLevel;

		public string LogSource => logSource;
		public string Text => text;

		public string StackTrace => stackTrace;

		public LogEntry(DateTime logTime, string logSource, LogLevel logLevel, string text, string stackTrace) {
			this.logTime = logTime;
			this.logSource = logSource;
			this.logLevel = logLevel;
			this.text = text;
			this.stackTrace = stackTrace;
			count = 1;
		}

		public bool ContentEquals(LogEntry other) {
			return ContentEquals(other.logSource, other.logLevel, other.text, other.stackTrace);
		}
		
		public bool ContentEquals(string otherLogSource, LogLevel otherLogLevel, string otherText, string otherStackTrace) {
			return logSource == otherLogSource 
					&& logLevel == otherLogLevel 
					&& text == otherText 
					&& stackTrace == otherStackTrace;
		}

		public LogEntry WithIncrementedCount() {
			return WithCount(count+1);
		}

		public LogEntry WithCount(int count) {
			LogEntry log = new LogEntry(logTime, logSource, logLevel, text, stackTrace);
			log.count = count;
			return log;
		}
	}
}