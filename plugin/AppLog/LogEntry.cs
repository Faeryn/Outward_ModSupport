using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace ModSupport.AppLog {
	[DataContract]
	public class LogEntry {
		[DataMember(Name="logTime")]
		public string LogTimeString {
			get => logTime.ToString("o", System.Globalization.CultureInfo.InvariantCulture);
			set => logTime = DateTime.Parse(value);
		}
		private DateTime logTime;
		
		[DataMember(Name="logType")]
		public string LogTypeString {
			get => logType.ToString();
			set => Enum.TryParse(value, out logType);
		}

		private LogType logType;
		
		[DataMember]
		private string text;
		
		[DataMember]
		private string stackTrace;


		public LogEntry(DateTime logTime, LogType logType, string text, string stackTrace) {
			this.logTime = logTime;
			this.logType = logType;
			this.text = text;
			this.stackTrace = stackTrace;
		}
	}
}