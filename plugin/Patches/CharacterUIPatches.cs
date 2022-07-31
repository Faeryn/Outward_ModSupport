using HarmonyLib;
using ModSupport.Extensions;

namespace ModSupport.Patches {
	[HarmonyPatch(typeof(CharacterUI))]
	public class CharacterUIPatches {
		[HarmonyPatch(nameof(CharacterUI.Awake)), HarmonyPostfix]
		static void CharacterUI_Awake_Postfix(CharacterUI __instance) {
			__instance.SetCurrentCharacterUI();
		}
	}
}