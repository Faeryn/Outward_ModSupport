using ExitGames.Client.Photon;
using HarmonyLib;
using ModSupport.UI;

namespace ModSupport.Patches {
	
	[HarmonyPatch(typeof(NetworkInstantiateManager))]
	public static class NetworkInstantiateManagerPatches {

		[HarmonyPatch(nameof(NetworkInstantiateManager.AddLocalPlayer)), HarmonyPostfix]
		private static void NetworkInstantiateManager_AddLocalPlayer_Postfix(PlayerSystem __instance) {
			ModList modList = ModList.FromPluginInfos();
			ModSupport.Log.LogDebug($"Local mod list: {modList}");
			Hashtable customProperties = new Hashtable();
			customProperties.Add("modlist", modList.ToDictArray());
			PhotonNetwork.player.SetCustomProperties(customProperties);
		}

	}
}