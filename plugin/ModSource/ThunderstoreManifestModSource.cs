using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using UnityEngine;

namespace ModSupport.ModSource {
	public class ThunderstoreManifestModSource : IModSource {
		public IEnumerable<ModInfo> GetMods() {
			List<ModInfo> mods = new List<ModInfo>();
			foreach (string dir in Directory.GetDirectories(Paths.PluginPath)) {
				string manifestJson = Path.Combine(dir, "manifest.json");
				if (!File.Exists(manifestJson)) {
					continue;
				}
				string json = File.ReadAllText(manifestJson);
				try {
					Manifest manifest = JsonUtility.FromJson<Manifest>(json);
					mods.Add(new ModInfo(null, manifest.name, Version.Parse(manifest.version_number)));
				} catch (Exception ex) {
					ModSupport.Log.LogError($"Failed to parse manifest.json: {ex} - '{json}'");
				}
			}
			return mods;
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