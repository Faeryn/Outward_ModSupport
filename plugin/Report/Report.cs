using System.Collections.Generic;
using System.Runtime.Serialization;
using ModSupport.AppLog;

namespace ModSupport.Report {
	[DataContract]
	public class Report {
		[DataMember] private string note;
		[DataMember] private List<ReportMod> mods;
		[DataMember] private List<LogEntry> logEntries;

		public Report(string note, IEnumerable<ReportMod> mods, IEnumerable<LogEntry> logEntries) {
			this.note = note;
			this.mods = new List<ReportMod>(mods);
			this.logEntries = new List<LogEntry>(logEntries);
		}
	}
}