using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ModSupport.UI {
	public static class UIHelper {
		private static GameObject modListMenu;
		
		public static void Initialize() {
			OptionsPanel optionsPanel = UIUtilities.GetOptionsPanelPrefab();
			modListMenu = new GameObject {
				name = "ModListMenu"
			};
			GameObject windowObj = Object.Instantiate(optionsPanel.transform.Find("Window").gameObject, modListMenu.transform);
			windowObj.name = "Window";
			modListMenu.AddComponent<ModListMenu>();
			Object.Destroy(windowObj.transform.FindInAllChildren("MappingContent").gameObject);
			Object.Destroy(windowObj.transform.FindInAllChildren("Tabs").gameObject);
			Text text = windowObj.transform.FindInAllChildren("lblTitle").GetComponent<Text>();
			Object.Destroy(text.GetComponent<UILocalize>());
			text.text = "Mods";
			GameObject footer = windowObj.transform.FindInAllChildren("NormalFooter").gameObject;
			FooterButtonHolder footerButtonHolder = footer.GetComponent<FooterButtonHolder>();
			footerButtonHolder.CancelInputDisplay.m_event = new Button.ButtonClickedEvent();
			Object.Destroy(footerButtonHolder.InfoInputDisplay.gameObject);
			GameObject content = windowObj.transform.FindInAllChildren("Viewport").Find("Content").gameObject;
			foreach (Transform child in content.transform) {
				Object.Destroy(child.gameObject);
			}
			Transform panel = windowObj.transform.Find("Panel");
			GameObject headers = new GameObject {
				name = "Headers"
			};
			headers.AddComponent<HorizontalLayoutGroup>();
			headers.transform.SetParent(panel, false);
			headers.transform.SetSiblingIndex(0);
			RectTransform rt = headers.GetComponent<RectTransform>();
			rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ModListMenu.PanelWidth-ModListMenu.HeaderOffset);
			CreateHeader("Mod name", ModListMenu.ModNameWidth, headers.transform);
			CreateHeader("Host ver.", "FirstVersionHeader", ModListMenu.HostVersionWidth, headers.transform);
			CreateHeader("Client ver.", "SecondVersionHeader", ModListMenu.ClientVersionWidth, headers.transform);
			CreateHeader("Status", "StatusHeader", ModListMenu.StatusWidth, headers.transform);
			rt.localPosition = new Vector3(-ModListMenu.HeaderOffset, 115, 0);
			RectTransform contentRT = content.GetComponent<RectTransform>();
			contentRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ModListMenu.PanelWidth);
		}

		public static GameObject GetModListMenuPrefab() {
			return modListMenu;
		}
		
		public static void AddButton(GameObject baseButton, string name, int index, string text, UnityAction onClick) {
			GameObject newButton = Object.Instantiate(baseButton, baseButton.transform.parent);
			newButton.name = name;
			newButton.transform.SetSiblingIndex(index);
			Text textComponent = newButton.GetComponentInChildren<Text>();
			Object.Destroy(textComponent.GetComponent<UILocalize>());
			textComponent.text = text;
			Button button = newButton.GetComponent<Button>();
			button.onClick = new Button.ButtonClickedEvent();
			button.onClick.AddListener(onClick);
		}
		
		
		public static Text CreateText(string text, float width, Transform parent, Color color) {
			Text modDisplay = CreateText(text, width, parent);
			modDisplay.color = color;
			return modDisplay;
		}

		public static Text CreateText(string text, float width, Transform parent) {
			GameObject modObj = new GameObject {
				name = "Text"
			};
			LayoutElement layoutElement = modObj.AddComponent<LayoutElement>();
			layoutElement.preferredWidth = width;
			Text textObj = modObj.AddComponent<Text>();
			textObj.text = text;
			textObj.font = UIUtilities.RegularFont;
			textObj.fontSize = 20;
			modObj.transform.SetParent(parent, false);
			return textObj;
		}

		public static Text CreateHeader(string text, string name, float width, Transform parent) {
			Text textObj = CreateHeader(text, width, parent);
			textObj.gameObject.name = name;
			return textObj;
		}

		public static Text CreateHeader(string text, float width, Transform parent) {
			Text textObj = CreateText(text, width, parent);
			textObj.fontSize = 22;
			return textObj;
		}

		
	}
}