using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ModSupport.UI {
	public static class UIHelper {

		public static void AddButton(GameObject baseButton, string name, int index, string textKey, UnityAction onClick) {
			GameObject newButton = Object.Instantiate(baseButton, baseButton.transform.parent);
			newButton.name = name;
			newButton.transform.SetSiblingIndex(index);
			Text textComponent = newButton.GetComponentInChildren<Text>();
			textComponent.GetComponent<UILocalize>().Key = textKey;
			textComponent.text = LocalizationManager.Instance.GetLoc(textKey);
			Button button = newButton.GetComponent<Button>();
			button.onClick = new Button.ButtonClickedEvent();
			button.onClick.AddListener(onClick);
		}

		public static Text CreateText(string textKey, float width, Transform parent, Color color) {
			Text modDisplay = CreateText(textKey, width, parent);
			modDisplay.color = color;
			return modDisplay;
		}

		public static Text CreateText(string textKey, float width, Transform parent) {
			GameObject modObj = new GameObject {
				name = "Text"
			};
			LayoutElement layoutElement = modObj.AddComponent<LayoutElement>();
			layoutElement.preferredWidth = width;
			Text textObj = modObj.AddComponent<Text>();
			modObj.AddComponent<UILocalize>().Key = textKey;
			textObj.text = LocalizationManager.Instance.GetLoc(textKey);
			textObj.font = UIUtilities.RegularFont;
			textObj.fontSize = 20;
			modObj.transform.SetParent(parent, false);
			return textObj;
		}

		public static Text CreateHeader(string textKey, string name, float width, Transform parent) {
			Text textObj = CreateHeader(textKey, width, parent);
			textObj.gameObject.name = name;
			return textObj;
		}

		public static Text CreateHeader(string textKey, float width, Transform parent) {
			Text textObj = CreateText(textKey, width, parent);
			textObj.fontSize = 22;
			return textObj;
		}
	}
}