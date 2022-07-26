using UnityEngine.Events;

namespace ModSupport.UI {
	public class ModSupportMenus {
		
		private const string SendReportQuestion = "Send error report to the ModSupport server to help fixing mod errors?";
		private const string ReportContents = "The report consists of your game log (the contents of output_log.txt) and the list of mods you are using (and nothing else).";
		private const string ReportQuestionAndContents = SendReportQuestion+" \n("+ReportContents+")";

		public static void ShowSendReportOnExitMsgBox(UnityAction callbackAfterReport = null, UnityAction callbackOnCancel = null) {
			MenuManager.Instance.ShowMessageBoxOkCancel(null, "Some errors happened while the game was running. "+ReportQuestionAndContents,
				() => {
					ModSupport.ReportManager.SendReport(callbackAfterReport, true);
				},
				() => {
					callbackOnCancel?.Invoke();
				},
				true);
		}
		
		public static void ShowSendReportMsgBox() {
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
			MenuManager.Instance.ShowMessageBoxOk(null, "You have already sent a report recently. Please wait a few minutes before sending another!", callbackAfterReport);
		}

		public static void ShowReportSentMsgBox(UnityAction callbackAfterReport = null) {
			MenuManager.Instance.ShowMessageBoxOk(null, "Report sent!", callbackAfterReport);
		}

		public static void ShowReportFailServerOverloadedMsgBox(UnityAction callbackAfterReport = null) {
			MenuManager.Instance.ShowMessageBoxOk(null, "Failed to send report because the server is overloaded at the moment. Please try again later!", callbackAfterReport);
		}

		public static void ShowReportFailServerInternalErrorMsgBox(UnityAction callbackAfterReport = null) {
			MenuManager.Instance.ShowMessageBoxOk(null, "Failed to send report because of an internal server error.", callbackAfterReport);
		}

		public static void ShowReportFailConnectionErrorMsgBox(UnityAction callbackAfterReport = null) {
			MenuManager.Instance.ShowMessageBoxOk(null, "Failed to send report due to connection error.", callbackAfterReport);
		}
	}
}