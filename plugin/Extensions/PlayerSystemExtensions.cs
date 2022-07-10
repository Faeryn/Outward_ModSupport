using System.Runtime.CompilerServices;

namespace ModSupport.Extensions {
	public static class PlayerSystemExtensions {
		private static ConditionalWeakTable<PlayerSystem, PlayerSystemExt> PlayerSystemExts = new ConditionalWeakTable<PlayerSystem, PlayerSystemExt>();

		private static PlayerSystemExt Ext(this PlayerSystem playerSystem) {
			if (!PlayerSystemExts.TryGetValue(playerSystem, out PlayerSystemExt ext)) {
				ext = new PlayerSystemExt();
				PlayerSystemExts.Add(playerSystem, ext);
			}
			return ext;
		}
		
		public static void SetModList(this PlayerSystem playerSystem, ModList modList) {
			playerSystem.Ext().ModList = modList;
		}
		
		public static ModList GetModList(this PlayerSystem playerSystem) {
			return playerSystem.Ext().ModList;
		}
		
		public static void SetModListViewed(this PlayerSystem playerSystem, bool val) {
			playerSystem.Ext().ModListViewed = val;
		}
		
		public static bool GetModListViewed(this PlayerSystem playerSystem) {
			return playerSystem.Ext().ModListViewed;
		}
	}
}