using System;

namespace ModSupport.Extensions {
	public static class CharacterUIExtensions {
		
		private static WeakReference<CharacterUI> currentCharacterUI = new WeakReference<CharacterUI>(null);

		public static void SetCurrentCharacterUI(this CharacterUI characterUI) {
			currentCharacterUI.SetTarget(characterUI);
		}
		
		public static bool TryGetCurrentCharacterUI(out CharacterUI characterUI) {
			characterUI = CharacterManager.Instance.GetFirstLocalCharacter()?.CharacterUI;
			if (characterUI) {
				return true;
			}
			return currentCharacterUI.TryGetTarget(out characterUI);
		}
		
		public static CharacterUI GetCurrentCharacterUI() {
			CharacterUI characterUI = CharacterManager.Instance.GetFirstLocalCharacter()?.CharacterUI;
			if (characterUI) {
				return characterUI;
			}
			if (TryGetCurrentCharacterUI(out characterUI)) {
				return characterUI;
			}
			return null;
		}
	}
}