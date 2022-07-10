using System;
using System.Collections.Generic;
using BepInEx;
using ExitGames.Client.Photon;
using Version = System.Version;

namespace ModSupport {
	public class ModInfo {
		public string GUID { get; private set; }
		public string Name { get; private set; }
		public Version Version { get; private set; }

		public ModInfo(string guid, string name, Version version) {
			GUID = guid;
			Name = name;
			Version = version;
		}

		public static ModInfo FromBepInPlugin(BepInPlugin plugin) {
			return new ModInfo(plugin.GUID, plugin.Name, plugin.Version);
		}

		/**
		 * Used for Photon serialization
		 */
		public Hashtable ToDict() {
			Hashtable dict = new Hashtable();
			dict.Add("GUID", GUID);
			dict.Add("Name", Name);
			dict.Add("Version", Version.ToString());
			return dict;
		}

		/**
		 * Used for Photon deserialization
		 */
		public static ModInfo FromDict(Hashtable dict) {
			return new ModInfo((string)dict["GUID"], (string)dict["Name"], Version.Parse((string)dict["Version"]));
		}
	}
}