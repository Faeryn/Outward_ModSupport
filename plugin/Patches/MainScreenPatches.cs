using System;
using System.Collections.Generic;
using HarmonyLib;
using ModSupport.UI;
using UnityEngine;

namespace ModSupport.Patches {
	
	[HarmonyPatch(typeof(MainScreen))]
	public static class MainScreenPatches {

		private static bool quitLatch = false;
		
		[HarmonyPatch(nameof(MainScreen.StartInit)), HarmonyPostfix]
		private static void MainScreen_StartInit_Postfix(MainScreen __instance) {
			
			ModListMenu modListMenu = ModListMenu.CreateEmpty(__instance.CharacterUI, __instance.transform);

			Transform buttonsPanelTransform = __instance.m_optionButtonsPanel.transform;
			GameObject creditsButton = buttonsPanelTransform.Find("btnCredit").gameObject;
			
			UIHelper.AddButton(creditsButton, "btnMods", 6, "Mods", () => {
				List<ModInfo> testModInfos = new List<ModInfo>();
				testModInfos.Add(new ModInfo("com.sinai.SideLoader", "SideLoader", Version.Parse("3.8.3")));
				testModInfos.Add(new ModInfo("faeryn.weaponskillkeys", "WeaponSkillKeys", Version.Parse("1.4.2")));
				testModInfos.Add(new ModInfo("faeryn.travelspeed", "TravelSpeed", Version.Parse("1.1.0")));
				testModInfos.Add(new ModInfo("test.TestMod", "TestMod", Version.Parse("2.2.0")));
				testModInfos.Add(new ModInfo("test2.TestMod2", "TestMod2", Version.Parse("3.2.0")));
				modListMenu.MenuMode = ModListMenuMode.Single;
				modListMenu.HostModList = ModSupport.Instance.ModListManager.ModList;
				modListMenu.ClientModList = new ModList(testModInfos);
				modListMenu.Show();
			});
		}
		
		[HarmonyPatch(nameof(MainScreen.Quit)), HarmonyPrefix]
		private static bool MainScreen_Quit_Prefix(MainScreen __instance) {
			if (ModSupport.ShowMsgBoxOnExceptionExit.Value && !quitLatch) {
				quitLatch = true;
				if (ModSupport.LogHandler.HasExceptions) {
					ModSupportMenus.ShowSendReportOnExitMsgBox(__instance.Quit, __instance.Quit);
					return false;
				}
			}
			return true;
		}
		
		[HarmonyPatch(nameof(MainScreen.OnHide)), HarmonyPostfix]
		private static void MainScreen_OnHide_Postfix(MainScreen __instance) {
			if (!__instance.m_started) {
				return;
			}
			ModListMenu modListMenu = __instance.GetComponentInChildren<ModListMenu>();
			if (modListMenu != null && modListMenu.IsDisplayed) {
				modListMenu.Hide();
			}
		}
		
		[HarmonyPatch(nameof(MainScreen.OnCancelInput)), HarmonyPrefix]
		private static void MainScreen_OnCancelInput_Prefix(MainScreen __instance) {
			ModListMenu modListMenu = __instance.GetComponentInChildren<ModListMenu>();
			if (modListMenu != null && modListMenu.IsDisplayed) {
				modListMenu.OnCancelInput();
			}
		}
	}
}