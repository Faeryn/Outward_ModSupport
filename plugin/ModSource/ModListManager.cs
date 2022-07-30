using System.Collections.Generic;
using System.Linq;

namespace ModSupport.ModSource {
	public class ModListManager {
		private List<IModSource> modSources = new List<IModSource> {
			new ThunderstoreManifestModSource(),
			new BepInExModSource()
		};

		private ModList modList;

		public ModList ModList => modList;

		internal void Initialize() {
			Dictionary<string, LocalMod> mods = new Dictionary<string, LocalMod>();
			foreach (IModSource modSource in modSources) {
				LoadMods(modSource, mods);
			}
			modList = new ModList(mods.Values.Select(mod => mod.ModInfo));
		}

		private void LoadMods(IModSource modSource, Dictionary<string, LocalMod> mods) {
			foreach (LocalMod newMod in modSource.GetMods()) {
				string modKey = newMod.FolderName;
				if (modKey == null) {
					modKey = newMod.DllName;
				}
				if (modKey == null) {
					// Fallback option in case folderName or dllName is not available for some reason
					// Not ideal but at this point it should be fine
					modKey = newMod.ModInfo.Name;
				}
				if (modKey == null) {
					ModSupport.Log.LogError($"Unable to determine mod root path or name for mod: {newMod.ModInfo}");
					continue;
				}
				if (!mods.ContainsKey(modKey)) {
					ModSupport.Log.LogDebug($"Adding mod: '{modKey}' - {newMod}");
					mods.Add(modKey, newMod);
				}
			}
		}
	}
}