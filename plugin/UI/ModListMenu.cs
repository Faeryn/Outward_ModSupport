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
		
		private InputDisplay sendReportButton;
		private FooterButtonHolder footerButtonHolder;
		
		private Text firstVersionHeader;
		private Text secondVersionHeader;
		private Text statusHeader;
		private Text errorsDisplay;
		private Text exceptionsDisplay;
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
			text.text = Loc("title");
			text.GetComponent<UILocalize>().Key = LocKey("title");
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
			UIHelper.CreateHeader(LocKey("header.mod_name"), ModNameWidth, headers.transform);
			UIHelper.CreateHeader(LocKey("header.host_version"), "FirstVersionHeader", HostVersionWidth, headers.transform);
			UIHelper.CreateHeader(LocKey("header.client_version"), "SecondVersionHeader", ClientVersionWidth, headers.transform);
			UIHelper.CreateHeader(LocKey("header.status"), "StatusHeader", StatusWidth, headers.transform);
			rt.localPosition = new Vector3(-HeaderOffset, 115, 0);
			RectTransform contentRT = content.GetComponent<RectTransform>();
			contentRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, PanelWidth);
			
			// Footer
			Text exceptionsText = UIHelper.CreateText(LocKey("footer.exceptions"), 200f, footer.transform, Color.green);
			exceptionsText.name = "ExceptionsDisplay";
			exceptionsText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200f);
			
			Text errorsText = UIHelper.CreateText(LocKey("footer.errors"), 120f, footer.transform, Color.green);
			errorsText.name = "ErrorsDisplay";
			errorsText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 120f);

			InputDisplay errorReportButton = footerButtonHolder.InfoInputDisplay;
			errorReportButton.m_event = new Button.ButtonClickedEvent();
			errorReportButton.m_lblActionText.GetComponent<UILocalize>().Key = LocKey("action.send_report");
			errorReportButton.ActionText = Loc("action.send_report");
			return modListMenuObj;
		}
		
		public override void StartInit() {
			base.StartInit();
			GameObject footer = transform.FindInAllChildren("NormalFooter").gameObject;
			footerButtonHolder = footer.GetComponent<FooterButtonHolder>();
			footerButtonHolder.CancelInputDisplay.m_event.AddListener(() => {
				Hide();
			});
			sendReportButton = footerButtonHolder.InfoInputDisplay;
			sendReportButton.m_event.AddListener(ModSupportMenus.ShowSendReportMsgBox);
			GameObject viewport = transform.FindInAllChildren("Viewport").gameObject;
			content = viewport.transform.Find("Content").gameObject;
			firstVersionHeader = transform.FindInAllChildren("FirstVersionHeader").GetComponent<Text>();
			secondVersionHeader = transform.FindInAllChildren("SecondVersionHeader").GetComponent<Text>();
			statusHeader = transform.FindInAllChildren("StatusHeader").GetComponent<Text>();
			errorsDisplay = transform.FindInAllChildren("ErrorsDisplay").GetComponent<Text>();
			exceptionsDisplay = transform.FindInAllChildren("ExceptionsDisplay").GetComponent<Text>();
		}

		public override void Show() {
			base.Show();
			Refresh();
		}

		private void Refresh() {
			RefreshModListDisplay();
			RefreshFooter();
		}

		private void SetText(Text text, string key, params string[] args) {
			text.GetComponent<UILocalize>().Key = args.Length == 0 && key != "" ? LocKey(key) : "";
			text.text = key == "" ? "" : Loc(key, args);
		}

		private void RefreshModListDisplay() {
			ResetModListDisplay();
			
			ModList modListA = null;
			ModList modListB = null;

			switch (MenuMode) {
				case ModListMenuMode.Single: {
					SetText(firstVersionHeader, "header.version");
					SetText(secondVersionHeader, "");
					SetText(statusHeader, "");
					modListA = HostModList ?? ClientModList;
					modListB = ModList.Empty;
					break;
				}
				case ModListMenuMode.MultiplayerCompare: {
					SetText(firstVersionHeader, "header.host_version");
					SetText(secondVersionHeader, "header.client_version");
					SetText(statusHeader, "header.status");
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
			if (ModSupport.OnlineEnabled.Value) {
				footerButtonHolder.m_infoInputDisplay = sendReportButton;
				sendReportButton.gameObject.SetActive(true);
			} else {
				footerButtonHolder.m_infoInputDisplay = null;
				sendReportButton.Hide();
				sendReportButton.gameObject.SetActive(false);
			}
			RefreshErrorDisplays();
		}

		private void RefreshErrorDisplays() {
			int numErrors = ModSupport.LogHandler.NumErrors;
			int numExceptions = ModSupport.LogHandler.NumExceptions;
			
			if (ModSupport.ErrorsAdvancedMode.Value) {
				errorsDisplay.gameObject.SetActive(true);
				SetText(errorsDisplay, "footer.errors", numErrors.ToString());
				SetText(exceptionsDisplay, "footer.exceptions", numExceptions.ToString());
				
			} else {
				errorsDisplay.gameObject.SetActive(false);
				SetText(exceptionsDisplay, "footer.errors", numExceptions.ToString());
			}
			
			if (numExceptions > 0) {
				exceptionsDisplay.color = Color.red;
			} else {
				exceptionsDisplay.color = Color.green;
			}
			
			if (numErrors > 0) {
				errorsDisplay.color = Color.yellow;
			} else {
				errorsDisplay.color = Color.green;
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
					status = LocKey("status.missing");
				} else if (hostVersion == null) {
					color = ExtraColor;
					status = LocKey("status.extra");
				} else if (hostVersion != clientVersion) {
					color = VersionMismatchColor;
					status = LocKey("status.wrong_version");
				} else {
					color = ModNameColor;
					status = LocKey("status.ok");
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
		
		private static string Loc(string key, params string[] args) {
			return LocalizationManager.Instance.GetLoc(LocKey(key), args);
		}
		
		private static string LocKey(string key) {
			return $"{ModSupport.GUID}.menu.modlist.{key}";
		}

	}
	
	public enum ModListMenuMode { Single, MultiplayerCompare }
	
}