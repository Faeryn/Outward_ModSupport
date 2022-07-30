using ExitGames.Client.Photon;
using Version = System.Version;

namespace ModSupport {
	public class ModInfo {
		public string GUID { get; }
		public string Name { get; }
		public Version Version { get; }
		public string Author { get; }
		public string Description { get; }
		public string WebsiteURL { get; }

		public ModInfo(string guid, string name, Version version, string author, string description, string websiteURL) {
			GUID = guid;
			Name = name;
			Version = version;
			Author = author;
			Description = description;
			WebsiteURL = websiteURL;
		}

		/**
		 * Used for Photon serialization
		 */
		public Hashtable ToDict() {
			Hashtable dict = new Hashtable();
			dict.Add("GUID", GUID);
			dict.Add("Name", Name);
			dict.Add("Version", Version.ToString());
			dict.Add("Author", Author);
			dict.Add("Description", Description);
			dict.Add("WebsiteURL", WebsiteURL);
			return dict;
		}

		/**
		 * Used for Photon deserialization
		 */
		public static ModInfo FromDict(Hashtable dict) {
			return new ModInfo((string)dict["GUID"], 
				(string)dict["Name"], 
				Version.Parse((string)dict["Version"]),
				(string)dict["Author"],
				(string)dict["Description"],
				(string)dict["WebsiteURL"]
				);
		}

		public override string ToString() {
			return $"ModInfo{{{nameof(GUID)}: {GUID}, {nameof(Name)}: {Name}, {nameof(Version)}: {Version}, {nameof(Author)}: {Author}}}";
		}
	}
}