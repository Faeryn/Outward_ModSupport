using ModSupport.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace ModSupport.UI {
	public class ModSupportMenus {
		
		private static string Loc(params string[] keys) {
			string result = "";
			foreach (string key in keys) {
				result += LocalizationManager.Instance.GetLoc($"{ModSupport.GUID}.msgbox.{key}");
				result += " ";
			}
			return result.Trim();
		}

		public static void ShowSendReportOnExitMsgBox(UnityAction callbackAfterReport = null) {
			if (!ModSupport.OnlineEnabled.Value) {
				callbackAfterReport?.Invoke();
				return;
			}
			if (ModSupport.SilentSend.Value) {
				ModSupport.ReportManager.SendReport(callbackAfterReport, true);
				return;
			}
			MenuManager.Instance.ShowMessageBoxOkCancel(null, Loc("runtime_errors", "report_question", "report_contents"),
				() => {
					ModSupport.ReportManager.SendReport(callbackAfterReport, true);
				},
				() => {
					callbackAfterReport?.Invoke();
				},
				true);
		}
		
		public static void ShowSendReportOnLoadingMsgBox(UnityAction callbackAfterReport = null) {
			if (!ModSupport.OnlineEnabled.Value) {
				callbackAfterReport?.Invoke();
				return;
			}
			if (ModSupport.SilentSend.Value) {
				ModSupport.ReportManager.SendReport(callbackAfterReport, true);
				return;
			}
			MenuManager.Instance.ShowMessageBoxOkCancel(CharacterUIExtensions.GetCurrentCharacterUI(), Loc("loading_errors", "report_question", "report_contents"),
				() => {
					ModSupport.ReportManager.SendReport(callbackAfterReport, true);
				},
				() => {
					callbackAfterReport?.Invoke();
				},
				true);
		}
		
		public static void ShowSendReportMsgBox() {
			if (!ModSupport.OnlineEnabled.Value) {
				return;
			}
			if (ModSupport.SilentSend.Value) {
				ModSupport.ReportManager.SendReport();
				return;
			}
			MenuManager.Instance.ShowMessageBoxOkCancel(null, Loc("report_question", "report_contents"),
				() => {
					ModSupport.ReportManager.SendReport();
				},
				() => { 
					// Do nothing
				},
				true);
		}
		
		public static void ShowExceptionMsgBox() {
			if (!ModSupport.OnlineEnabled.Value || ModSupport.SilentSend.Value) {
				return;
			}
			MenuManager.Instance.ShowMessageBoxOkCancel(null, Loc("error", "report_question", "report_contents"),
				() => {
					ModSupport.ReportManager.SendReport();
				},
				() => { 
					// Do nothing
				},
				true);
		}

		public static void ShowAlreadySentReportMsgBox(UnityAction callbackAfterReport = null) {
			if (!ModSupport.OnlineEnabled.Value || ModSupport.SilentSend.Value) {
				callbackAfterReport?.Invoke();
				return;
			}
			MenuManager.Instance.ShowMessageBoxOk(null, Loc("report_already_sent"), callbackAfterReport);
		}

		public static void ShowReportSentMsgBox(UnityAction callbackAfterReport = null) {
			if (!ModSupport.OnlineEnabled.Value || ModSupport.SilentSend.Value) {
				callbackAfterReport?.Invoke();
				return;
			}
			MenuManager.Instance.ShowMessageBoxOk(null, Loc("report_sent"), callbackAfterReport);
		}

		public static void ShowReportFailServerOverloadedMsgBox(UnityAction callbackAfterReport = null) {
			if (!ModSupport.OnlineEnabled.Value || ModSupport.SilentSend.Value) {
				callbackAfterReport?.Invoke();
				return;
			}
			MenuManager.Instance.ShowMessageBoxOk(null, Loc("report_fail.busy"), callbackAfterReport);
		}

		public static void ShowReportFailServerInternalErrorMsgBox(UnityAction callbackAfterReport = null) {
			if (!ModSupport.OnlineEnabled.Value || ModSupport.SilentSend.Value) {
				callbackAfterReport?.Invoke();
				return;
			}
			MenuManager.Instance.ShowMessageBoxOk(null, Loc("report_fail.error"), callbackAfterReport);
		}

		public static void ShowReportFailConnectionErrorMsgBox(UnityAction callbackAfterReport = null) {
			if (!ModSupport.OnlineEnabled.Value || ModSupport.SilentSend.Value) {
				callbackAfterReport?.Invoke();
				return;
			}
			MenuManager.Instance.ShowMessageBoxOk(null, Loc("report_fail.connection"), callbackAfterReport);
		}

		public static void ShowExitPrompt() {
			MenuManager.Instance.ShowMessageBoxOkCancel(null, Loc("msgbox.loading_errors", "msgbox.exit"), Application.Quit, null, true);
		}
	}
}