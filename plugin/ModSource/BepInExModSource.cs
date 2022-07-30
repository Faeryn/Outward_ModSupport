using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Bootstrap;

namespace ModSupport.ModSource {
	public class BepInExModSource : IModSource {
		public IEnumerable<LocalMod> GetMods() {
			List<LocalMod> mods = new List<LocalMod>();
			string modsRoot = Paths.PluginPath;

			foreach (PluginInfo info in Chainloader.PluginInfos.Values) {
				string dllPath = info.Location;
				string folderName = GetFolderName(modsRoot, dllPath);
				mods.Add(new LocalMod(FromBepInPlugin(info.Metadata), Path.GetFileName(dllPath), folderName));
			}
			return mods;
		}

		private string GetFolderName(string modsRoot, string dllPath) {
			if (!dllPath.StartsWith(modsRoot)) {
				return null;
			}
			string rootless = dllPath.Replace(modsRoot, "");
			if (rootless.StartsWith(Path.DirectorySeparatorChar.ToString())) {
				rootless = rootless.Substring(1);
			}
			return rootless.Split(Path.DirectorySeparatorChar)[0];
		}

		private ModInfo FromBepInPlugin(BepInPlugin plugin) {
			return new ModInfo(plugin.GUID, plugin.Name, plugin.Version, null, null, null);
		}
	}
}