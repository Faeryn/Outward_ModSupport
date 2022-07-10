using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ModSupport.UI {
	public class ModListMenu : Panel {
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

		public string HostPlayerName { get; set; }
		public string ClientPlayerName { get; set; }
		public ModList HostModList { get; set; }
		public ModList ClientModList { get; set; }
		
		private Text title;
		private GameObject content;

		public override void AwakeInit() {
			base.AwakeInit();
			title = transform.FindInAllChildren("lblTitle").GetComponent<Text>();
			GameObject footer = transform.FindInAllChildren("NormalFooter").gameObject;
			FooterButtonHolder footerButtonHolder = footer.GetComponent<FooterButtonHolder>();
			footerButtonHolder.CancelInputDisplay.m_event.AddListener(() => {
				Hide();
			});
			GameObject viewport = transform.FindInAllChildren("Viewport").gameObject;
			content = viewport.transform.Find("Content").gameObject;
		}

		public override void Show() {
			base.Show();
			Refresh();
		}

		private void Refresh() {
			ResetModListDisplay();
			if (HostModList == null) {
				HostModList = ModList.Empty;
			}
			if (ClientModList == null) {
				ClientModList = ModList.Empty;
			}
			HashSet<ModInfo> foundClientMods = new HashSet<ModInfo>();
			foreach (ModInfo modA in HostModList) {
				ModInfo modB = ClientModList.FindByGuid(modA.GUID);
				if (modB != null) {
					foundClientMods.Add(modB);
				}
				CreateModRow(modA.Name, modA.Version, modB?.Version, content.transform);
			}
			foreach (ModInfo modB in ClientModList) {
				if (!foundClientMods.Contains(modB)) {
					CreateModRow(modB.Name, null, modB.Version, content.transform);
				}
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
			
			UIHelper.CreateText(modName, ModNameWidth, row.transform, color);
			UIHelper.CreateText(hostVersion != null ? hostVersion.ToString() : "", HostVersionWidth, row.transform, color);
			UIHelper.CreateText(clientVersion != null ? clientVersion.ToString() : "", ClientVersionWidth, row.transform, color);
			UIHelper.CreateText(status, StatusWidth, row.transform, color);
			
			row.transform.SetParent(content.transform, false);
		}

		public static ModListMenu CreateEmpty(CharacterUI characterUI, Transform parent) {
			GameObject modListMenuObj = Object.Instantiate(UIHelper.GetModListMenuPrefab(), parent);
			modListMenuObj.name = "ModListMenu";
			ModListMenu list = modListMenuObj.GetComponent<ModListMenu>();
			list.SetCharacterUI(characterUI);
			return list;
		}

		public void SetTitle(string newTitle) {
			title.text = newTitle;
		}

	}
}