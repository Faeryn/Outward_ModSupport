using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using UnityEngine;

namespace ModSupport.ModSource {
	public class ThunderstoreManifestModSource : IModSource {
		public IEnumerable<LocalMod> GetMods() {
			List<LocalMod> mods = new List<LocalMod>();
			foreach (string dir in Directory.GetDirectories(Paths.PluginPath)) {
				string manifestJson = Path.Combine(dir, "manifest.json");
				if (!File.Exists(manifestJson)) {
					continue;
				}

				string folderName = Path.GetFileName(dir);
				string json = File.ReadAllText(manifestJson);
				try {
					Manifest manifest = JsonUtility.FromJson<Manifest>(json);
					string name = manifest.name;
					ModInfo modInfo = new ModInfo(null, name, Version.Parse(manifest.version_number),
						TryGetAuthor(folderName, name), manifest.description, manifest.website_url);
					mods.Add(new LocalMod(modInfo, null, folderName));
				} catch (Exception ex) {
					ModSupport.Log.LogError($"Failed to parse manifest.json: {ex} - '{json}'");
				}
			}
			return mods;
		}

		private string TryGetAuthor(string dirName, string modName) {
			if (!dirName.EndsWith(modName)) {
				return null;
			}
			string tmp = dirName.Replace(modName, "");
			if (!tmp.EndsWith("-")) {
				return null;
			}
			return tmp.Substring(0, tmp.Length - 1);
		}
	}

	[Serializable]
	internal class Manifest {
		public string name;
		public string version_number;
		public string website_url;
		public string description;
	}
}