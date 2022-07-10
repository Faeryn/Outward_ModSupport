using ExitGames.Client.Photon;
using HarmonyLib;
using ModSupport.Extensions;

namespace ModSupport.Patches {
	
	[HarmonyPatch(typeof(PlayerSystem))]
	public static class PlayerSystemPatches {
		
		[HarmonyPatch(nameof(PlayerSystem.SetPhotonOwner)), HarmonyPostfix]
		private static void PlayerSystem_SetPhotonOwner_Postfix(PlayerSystem __instance, PhotonPlayer _player) {
			if (_player.CustomProperties.ContainsKey("modlist")) {
				ModList modList = ModList.FromDictArray(_player.CustomProperties["modlist"] as Hashtable[]);
				ModSupport.Log.LogDebug($"Setting mod list for player {_player.NickName}: {modList}");
				__instance.SetModList(modList);
			} else {
				ModSupport.Log.LogDebug($"{_player.NickName} has no mod list");
			}
		}

	}
}