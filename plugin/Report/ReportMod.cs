using System.Runtime.Serialization;

namespace ModSupport.Report {
	[DataContract]
	public class ReportMod {
		[DataMember] private string guid;
		[DataMember] private string name;
		[DataMember] private string author;
		[DataMember] private int versionMajor;
		[DataMember] private int versionMinor;
		[DataMember] private int versionRevision;

		public ReportMod(string guid, string name, int versionMajor, int versionMinor, int versionRevision, string author) {
			this.guid = guid;
			this.name = name;
			this.versionMajor = versionMajor;
			this.versionMinor = versionMinor;
			this.versionRevision = versionRevision;
			this.author = author;
		}
	}
}