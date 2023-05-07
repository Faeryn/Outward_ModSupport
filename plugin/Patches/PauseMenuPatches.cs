using HarmonyLib;
using ModSupport.Extensions;
using ModSupport.UI;
using UnityEngine;

namespace ModSupport.Patches {
	
	[HarmonyPatch(typeof(PauseMenu))]
	public static class PauseMenuPatches {
		
		private static bool quitLatch = false;

		[HarmonyPatch(nameof(PauseMenu.StartInit)), HarmonyPostfix]
		private static void PauseMenu_StartInit_Postfix(PauseMenu __instance) {
			ModListMenu modListMenu = ModListMenu.CreateEmpty(__instance.CharacterUI, __instance.transform);
			Transform buttonsPanelTransform = __instance.m_hideOnPauseButtons.transform;
			GameObject settingsButton = buttonsPanelTransform.Find("btnOptions").gameObject;

			UIHelper.AddButton(settingsButton, "btnMods", 5, $"{ModSupport.GUID}.menu.modlist.title", () => {
				PlayerSystem host = GetHostPlayer();
				PlayerSystem client = GetFirstOtherPlayerOrSelf();
				modListMenu.MenuMode = Global.Lobby.PlayersInLobbyCount > 1 ? ModListMenuMode.MultiplayerCompare : ModListMenuMode.Single;
				modListMenu.HostModList = host != null ? host.GetModList() : null;
				modListMenu.ClientModList = client != null ? client.GetModList() : null;
				modListMenu.Show();
			});

			GameObject background = __instance.transform.FindInAllChildren("BG").gameObject;
			RectTransform bgTransform = background.GetComponent<RectTransform>();
			bgTransform.anchorMin -= new Vector2(0, 0.1f);
		}
		
				
		[HarmonyPatch(nameof(PauseMenu.OnHide)), HarmonyPostfix]
		private static void PauseMenu_OnHide_Postfix(PauseMenu __instance) {
			ModListMenu modListMenu = __instance.GetComponentInChildren<ModListMenu>();
			if (modListMenu != null && modListMenu.IsDisplayed) {
				modListMenu.Hide();
			}
		}
		
		[HarmonyPatch(nameof(PauseMenu.Quit)), HarmonyPrefix]
		private static bool PauseMenu_Quit_Prefix(PauseMenu __instance) {
			if (ModSupport.SendOnExit.Value && !quitLatch) {
				quitLatch = true;
				if (ModSupport.LogHandler.HasExceptions) {
					ModSupportMenus.ShowSendReportOnExitMsgBox(__instance.Quit);
					return false;
				}
			}
			return true;
		}

		[HarmonyPatch(nameof(PauseMenu.OnCancelInput)), HarmonyPrefix]
		private static void PauseMenu_OnCancelInput_Prefix(PauseMenu __instance) {
			ModListMenu modListMenu = __instance.GetComponentInChildren<ModListMenu>();
			if (modListMenu != null && modListMenu.IsDisplayed) {
				modListMenu.OnCancelInput();
			}
		}

		private static PlayerSystem GetHostPlayer() {
			foreach (PlayerSystem player in Global.Lobby.PlayersInLobby) {
				if (player.IsMasterClient) {
					return player;
				}
			}
			return null;
		}
		
		private static PlayerSystem GetFirstOtherPlayerOrSelf() {
			// If we are not host, return ourselves
			if (!Global.Lobby.IsWorldOwner) {
				return Global.Lobby.GetLocalPlayer(0);
			}
			// If we are the host, return the first client
			foreach (PlayerSystem player in Global.Lobby.PlayersInLobby) {
				if (!player.IsMasterClient) {
					return player;
				}
			}
			return null;
		}
	}
}