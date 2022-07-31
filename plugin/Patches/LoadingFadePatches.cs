using HarmonyLib;
using ModSupport.Extensions;
using ModSupport.UI;
using UnityEngine;

namespace ModSupport.Patches {
	[HarmonyPatch(typeof(LoadingFade))]
	public static class LoadingFadePatches {
		private static readonly int exceptionCheckInterval = 60;
		private static readonly int exceptionAlertThreshold = 10;
		
		private static bool msgBoxDisplayed = false;
		private static int exceptionsOnLoadStart;
				
		[HarmonyPatch(nameof(LoadingFade.StartLoading), typeof(string), typeof(float)), HarmonyPrefix]
		private static void LoadingFade_StartLoading_Postfix(LoadingFade __instance, string _title, float _delay) {
			exceptionsOnLoadStart = ModSupport.LogHandler.NumExceptions;
			msgBoxDisplayed = false;
		}
		
		
		[HarmonyPatch(nameof(LoadingFade.Update)), HarmonyPrefix]
		
		private static void LoadingFade_Update_Prefix(LoadingFade __instance) {
			if (msgBoxDisplayed || !__instance.m_loadingStarted || Time.frameCount % exceptionCheckInterval != 0) {
				return;
			}

			if (ModSupport.SendOnLoadingExceptions.Value && exceptionsOnLoadStart + exceptionAlertThreshold <= ModSupport.LogHandler.NumExceptions) {
				msgBoxDisplayed = true;
				ModSupportMenus.ShowSendReportOnLoadingMsgBox();
				if (CharacterUIExtensions.TryGetCurrentCharacterUI(out CharacterUI characterUI)) {
					characterUI.MessagePanel = MenuManager.m_instance.m_messageBox;
				}
			}
		}
	}
}