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
		}
	}
}