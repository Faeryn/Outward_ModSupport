using System.Collections.Generic;
using BepInEx;
using BepInEx.Bootstrap;

namespace ModSupport.ModSource {
	public class BepInExModSource : IModSource {
		public IEnumerable<ModInfo> GetMods() {
			List<ModInfo> modInfos = new List<ModInfo>();
			foreach (PluginInfo info in Chainloader.PluginInfos.Values) {
				modInfos.Add(ModInfo.FromBepInPlugin(info.Metadata));
			}
			return modInfos;
		}
	}
}