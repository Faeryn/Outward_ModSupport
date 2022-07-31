using ModSupport.Extensions;
using UnityEngine.Events;

namespace ModSupport.UI {
	public class ModSupportMenus {
		
		private const string SendReportQuestion = "Do you want to send an error report to the ModSupport server to help fix mod errors?";
		private const string ReportContents = "The report consists of nothing else but your game log (the contents of output_log.txt) and the list of mods you are using.";
		private const string ReportQuestionAndContents = SendReportQuestion + " " + ReportContents;

		public static void ShowSendReportOnExitMsgBox(UnityAction callbackAfterReport = null, UnityAction callbackOnCancel = null) {
			if (ModSupport.SilentSend.Value) {
				ModSupport.ReportManager.SendReport(null, true);
				return;
			}
			MenuManager.Instance.ShowMessageBoxOkCancel(null, "Some errors happened while the game was running. "+ReportQuestionAndContents,
				() => {
					ModSupport.ReportManager.SendReport(callbackAfterReport, true);
				},
				() => {
					callbackOnCancel?.Invoke();
				},
				true);
		}
		
		public static void ShowSendReportOnLoadingMsgBox() {
			if (ModSupport.SilentSend.Value) {
				ModSupport.ReportManager.SendReport(null, true);
				return;
			}
			MenuManager.Instance.ShowMessageBoxOkCancel(CharacterUIExtensions.GetCurrentCharacterUI(), "Too many errors while loading (possibly endless loading screen). "+ReportQuestionAndContents,
				() => {
					ModSupport.ReportManager.SendReport(null, true);
				},
				() => {
					// Do nothing
				},
				true);
		}
		
		public static void ShowSendReportMsgBox() {
			if (ModSupport.SilentSend.Value) {
				ModSupport.ReportManager.SendReport();
				return;
			}
			MenuManager.Instance.ShowMessageBoxOkCancel(null, ReportQuestionAndContents,
				() => {
					ModSupport.ReportManager.SendReport();
				},
				() => { 
					// Do nothing
				},
				true);
		}
		
		public static void ShowExceptionMsgBox() {
			if (ModSupport.SilentSend.Value) {
				return;
			}
			MenuManager.Instance.ShowMessageBoxOkCancel(null, "An error occurred! "+ReportQuestionAndContents,
				() => {
					ModSupport.ReportManager.SendReport();
				},
				() => { 
					// Do nothing
				},
				true);
		}

		public static void ShowAlreadySentReportMsgBox(UnityAction callbackAfterReport = null) {
			if (ModSupport.SilentSend.Value) {
				return;
			}
			MenuManager.Instance.ShowMessageBoxOk(null, "You have already sent a report recently. Please wait a few minutes before sending another!", callbackAfterReport);
		}

		public static void ShowReportSentMsgBox(UnityAction callbackAfterReport = null) {
			if (ModSupport.SilentSend.Value) {
				return;
			}
			MenuManager.Instance.ShowMessageBoxOk(null, "Report sent!", callbackAfterReport);
		}

		public static void ShowReportFailServerOverloadedMsgBox(UnityAction callbackAfterReport = null) {
			if (ModSupport.SilentSend.Value) {
				return;
			}
			MenuManager.Instance.ShowMessageBoxOk(null, "Failed to send report because the server is overloaded at the moment. Please try again later!", callbackAfterReport);
		}

		public static void ShowReportFailServerInternalErrorMsgBox(UnityAction callbackAfterReport = null) {
			if (ModSupport.SilentSend.Value) {
				return;
			}
			MenuManager.Instance.ShowMessageBoxOk(null, "Failed to send report because of an internal server error.", callbackAfterReport);
		}

		public static void ShowReportFailConnectionErrorMsgBox(UnityAction callbackAfterReport = null) {
			if (ModSupport.SilentSend.Value) {
				return;
			}
			MenuManager.Instance.ShowMessageBoxOk(null, "Failed to send report due to connection error.", callbackAfterReport);
		}
	}
}