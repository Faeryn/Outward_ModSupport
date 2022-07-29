using System.Collections.Generic;

namespace ModSupport.ModSource {
	public class ModListManager {
		private List<IModSource> modSources = new List<IModSource> {
			new BepInExModSource(),
			new ThunderstoreManifestModSource()
		};

		private ModList modList;

		public ModList ModList => modList;

		internal void Initialize() {
			List<ModInfo> mods = new List<ModInfo>();
			foreach (IModSource modSource in modSources) {
				LoadMods(modSource, mods);
			}
			modList = new ModList(mods);
		}

		private bool ContainsByName(List<ModInfo> mods, ModInfo modInfo) {
			foreach (ModInfo mod in mods) {
				if (modInfo.Name == mod.Name) {
					return true;
				}
			}
			return false;
		}

		private void LoadMods(IModSource modSource, List<ModInfo> mods) {
			foreach (ModInfo newMod in modSource.GetMods()) {
				if (!ContainsByName(mods, newMod)) {
					mods.Add(newMod);
				}
			}
		}
	}
}