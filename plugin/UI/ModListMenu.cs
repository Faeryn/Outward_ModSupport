using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ModSupport.UI {
	public class ModListMenu : Panel {
		private static GameObject ModListMenuPrefab;
		
		private readonly Color ModNameColor = Color.white;
		private readonly Color VersionMismatchColor = Color.yellow;
		private readonly Color MissingColor = Color.red;
		private readonly Color ExtraColor = Color.cyan;

		public const int NumColumns = 4;
		public const float HeaderOffset = 24f;
		public const float PanelWidth = 682.0f;
		public const float HorizontalSpacing = 5f;
		public const float EffectiveWidth = PanelWidth - HorizontalSpacing*(NumColumns-1);
		public const float ModNameWidth = EffectiveWidth * 0.4f;
		public const float HostVersionWidth = EffectiveWidth * 0.2f;
		public const float ClientVersionWidth = EffectiveWidth * 0.2f;
		public const float StatusWidth = EffectiveWidth * 0.2f;

		public ModListMenuMode MenuMode { get; set; } = ModListMenuMode.Single;
		
		public string HostPlayerName { get; set; }
		public string ClientPlayerName { get; set; }
		public ModList HostModList { get; set; }
		public ModList ClientModList { get; set; }
		
		private Text title;
		private Text firstVersionHeader;
		private Text secondVersionHeader;
		private Text statusHeader;
		private Text warningsErrorsDisplay;
		private GameObject content;

		internal static void InitializePrefab() {
			ModListMenuPrefab = BuildPrefab();
		}
		
		private static GameObject BuildPrefab() {
			// Clone options panel
			OptionsPanel optionsPanel = UIUtilities.GetOptionsPanelPrefab();
			GameObject modListMenuObj = new GameObject {
				name = "ModListMenu"
			};
			GameObject windowObj = Object.Instantiate(optionsPanel.transform.Find("Window").gameObject, modListMenuObj.transform);
			windowObj.name = "Window";
			modListMenuObj.AddComponent<ModListMenu>();
			
			// Remove options-related components, set title
			Object.Destroy(windowObj.transform.FindInAllChildren("MappingContent").gameObject);
			Object.Destroy(windowObj.transform.FindInAllChildren("Tabs").gameObject);
			Text text = windowObj.transform.FindInAllChildren("lblTitle").GetComponent<Text>();
			Object.Destroy(text.GetComponent<UILocalize>());
			text.text = "Mods";
			GameObject footer = windowObj.transform.FindInAllChildren("NormalFooter").gameObject;
			FooterButtonHolder footerButtonHolder = footer.GetComponent<FooterButtonHolder>();
			footerButtonHolder.CancelInputDisplay.m_event = new Button.ButtonClickedEvent();
			GameObject content = windowObj.transform.FindInAllChildren("Viewport").Find("Content").gameObject;
			foreach (Transform child in content.transform) {
				Object.Destroy(child.gameObject);
			}
						
			// Headers
			Transform panel = windowObj.transform.Find("Panel");
			GameObject headers = new GameObject {
				name = "Headers"
			};
			headers.AddComponent<HorizontalLayoutGroup>();
			headers.transform.SetParent(panel, false);
			headers.transform.SetSiblingIndex(0);
			RectTransform rt = headers.GetComponent<RectTransform>();
			rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, PanelWidth-HeaderOffset);
			UIHelper.CreateHeader("Mod name", ModNameWidth, headers.transform);
			UIHelper.CreateHeader("Host ver.", "FirstVersionHeader", HostVersionWidth, headers.transform);
			UIHelper.CreateHeader("Client ver.", "SecondVersionHeader", ClientVersionWidth, headers.transform);
			UIHelper.CreateHeader("Status", "StatusHeader", StatusWidth, headers.transform);
			rt.localPosition = new Vector3(-HeaderOffset, 115, 0);
			RectTransform contentRT = content.GetComponent<RectTransform>();
			contentRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, PanelWidth);
			
			// Footer
			Text warningsErrorsText = UIHelper.CreateText("0 warnings, 0 errors", 220f, footer.transform, Color.green);
			warningsErrorsText.name = "WarningsErrorsDisplay";
			warningsErrorsText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 220f);
			InputDisplay warningsErrorsButton = footerButtonHolder.InfoInputDisplay;
			warningsErrorsButton.m_event = new Button.ButtonClickedEvent();
			warningsErrorsButton.ActionText = "Copy errors to clipboard";
			warningsErrorsButton.m_isStaticInputText = true;
			return modListMenuObj;
		}
		
		public override void StartInit() {
			base.StartInit();
			title = transform.FindInAllChildren("lblTitle").GetComponent<Text>();
			GameObject footer = transform.FindInAllChildren("NormalFooter").gameObject;
			FooterButtonHolder footerButtonHolder = footer.GetComponent<FooterButtonHolder>();
			footerButtonHolder.CancelInputDisplay.m_event.AddListener(() => {
				Hide();
			});
			footerButtonHolder.InfoInputDisplay.m_event.AddListener(() => {
				ModSupport.LogHandler.CopyErrorsToClipboard();
			});
			GameObject viewport = transform.FindInAllChildren("Viewport").gameObject;
			content = viewport.transform.Find("Content").gameObject;
			firstVersionHeader = transform.FindInAllChildren("FirstVersionHeader").GetComponent<Text>();
			secondVersionHeader = transform.FindInAllChildren("SecondVersionHeader").GetComponent<Text>();
			statusHeader = transform.FindInAllChildren("StatusHeader").GetComponent<Text>();
			warningsErrorsDisplay = transform.FindInAllChildren("WarningsErrorsDisplay").GetComponent<Text>();
		}

		public override void Show() {
			base.Show();
			Refresh();
		}

		private void Refresh() {
			RefreshModListDisplay();
			RefreshFooter();
		}

		private void RefreshModListDisplay() {
			ResetModListDisplay();
			
			ModList modListA = null;
			ModList modListB = null;

			switch (MenuMode) {
				case ModListMenuMode.Single: {
					firstVersionHeader.text = "Version";
					secondVersionHeader.text = "";
					statusHeader.text = "";
					modListA = HostModList ?? ClientModList;
					modListB = ModList.Empty;
					break;
				}
				case ModListMenuMode.MultiplayerCompare: {
					firstVersionHeader.text = "Host ver.";
					secondVersionHeader.text = "Client ver.";
					statusHeader.text = "Status";
					modListA = HostModList;
					modListB = ClientModList;
					break;
				}
			}
			
			if (modListA == null) {
				modListA = ModList.Empty;
			}
			if (modListB == null) {
				modListB = ModList.Empty;
			}
			
			HashSet<ModInfo> foundClientMods = new HashSet<ModInfo>();
			foreach (ModInfo modA in modListA) {
				ModInfo modB = modListB.FindByGuid(modA.GUID);
				if (modB != null) {
					foundClientMods.Add(modB);
				}
				CreateModRow(modA.Name, modA.Version, modB?.Version, content.transform);
			}
			foreach (ModInfo modB in modListB) {
				if (!foundClientMods.Contains(modB)) {
					CreateModRow(modB.Name, null, modB.Version, content.transform);
				}
			}
		}

		private void RefreshFooter() {
			int numErrors = ModSupport.LogHandler.NumErrors;
			int numWarnings = ModSupport.LogHandler.NumWarnings;
			warningsErrorsDisplay.text = $"{numWarnings} warnings, {numErrors} errors";
			if (numErrors > 0) {
				warningsErrorsDisplay.color = Color.red;
			} else if (numWarnings > 0) {
				warningsErrorsDisplay.color = Color.yellow;
			} else {
				warningsErrorsDisplay.color = Color.white;
			}
		}
		
		private void ResetModListDisplay() {
			foreach (Transform child in content.transform) {
            	Object.Destroy(child.gameObject);
            }
		}

		private void CreateModRow(string modName, Version hostVersion, Version clientVersion, Transform parent) {
			GameObject row = new GameObject {
				name = "ModRow"
			};
			HorizontalLayoutGroup hlg = row.AddComponent<HorizontalLayoutGroup>();
			hlg.spacing = HorizontalSpacing;
			Color color;
			string status;
			if (MenuMode == ModListMenuMode.MultiplayerCompare) {
				if (clientVersion == null) {
					color = MissingColor;
					status = "Missing";
				} else if (hostVersion == null) {
					color = ExtraColor;
					status = "Extra";
				} else if (hostVersion != clientVersion) {
					color = VersionMismatchColor;
					status = "Wrong ver.";
				} else {
					color = ModNameColor;
					status = "OK";
				}
			} else {
				color = ModNameColor;
				status = "";
			}

			UIHelper.CreateText(modName, ModNameWidth, row.transform, color);
			UIHelper.CreateText(hostVersion != null ? hostVersion.ToString() : "", HostVersionWidth, row.transform, color);
			UIHelper.CreateText(clientVersion != null ? clientVersion.ToString() : "", ClientVersionWidth, row.transform, color);
			UIHelper.CreateText(status, StatusWidth, row.transform, color);
			
			row.transform.SetParent(content.transform, false);
		}

		public static ModListMenu CreateEmpty(CharacterUI characterUI, Transform parent) {
			GameObject modListMenuObj = Object.Instantiate(ModListMenuPrefab, parent);
			modListMenuObj.name = "ModListMenu";
			ModListMenu list = modListMenuObj.GetComponent<ModListMenu>();
			list.SetCharacterUI(characterUI);
			return list;
		}

		public void SetTitle(string newTitle) {
			title.text = newTitle;
		}

	}
	
	public enum ModListMenuMode { Single, MultiplayerCompare }
	
}