using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using ModSupport.AppLog;
using ModSupport.UI;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace ModSupport.Report {
	public class ReportManager {
		private const string ModSupportURL = "http://modsupport.net";
		
		private static readonly TimeSpan MinReportInterval = TimeSpan.FromMinutes(5);
		
		private DateTime lastReportTime = DateTime.MinValue;
		
		private Report GenerateReport() {
			List<ReportMod> mods = new List<ReportMod>();
			ModList modList = ModSupport.Instance.ModListManager.ModList;
			foreach (ModInfo modInfo in modList) {
				mods.Add(new ReportMod(modInfo.GUID, modInfo.Name, modInfo.Version.Major, modInfo.Version.Minor, modInfo.Version.Revision, modInfo.Author));
			}
			IEnumerable<LogEntry> logs = ModSupport.SendOnlyErrors.Value
				? ModSupport.LogHandler.GetCopyOfErrors()
				: ModSupport.LogHandler.GetCopyOfLogEntries();
			return new Report(null, mods, logs);
		}

		private byte[] ReportToJson(Report report) {
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Report));
			using (MemoryStream memoryStream = new MemoryStream()) {
				serializer.WriteObject(memoryStream, report);
				return memoryStream.ToArray();
			}
		}

		public void SendReport(UnityAction callbackAfterReport = null, bool ignoreReportInterval = false) {
			if (!ModSupport.OnlineEnabled.Value) {
				return;
			}
			
			if (!ignoreReportInterval && lastReportTime + MinReportInterval > DateTime.Now) {
				ModSupportMenus.ShowAlreadySentReportMsgBox(callbackAfterReport);
				return;
			}
			MenuManager.Instance.ShowConnectionScreen(LocalizationManager.Instance.GetLoc($"{ModSupport.GUID}.msgbox.sending_report"));
			Report report = GenerateReport();
			ModSupport.Instance.StartCoroutine(SendReportSync(report, callbackAfterReport));
		}

		private IEnumerator SendReportSync(Report report, UnityAction callbackAfterReport) {
			byte[] reportJson = ReportToJson(report);
			using (UnityWebRequest request = new UnityWebRequest($"{ModSupportURL}/api/v1/report", "POST")) {
				request.SetRequestHeader("Content-Type", "application/json");
				request.SetChunked(true);
				request.uploadHandler = new UploadHandlerRaw(reportJson);
				request.downloadHandler = new DownloadHandlerBuffer();
				yield return request.SendWebRequest();
				long responseCode = request.responseCode;
				UnityWebRequest.Result result = request.result;
				MenuManager.Instance.ShowConnectionScreen(false);
				if (result == UnityWebRequest.Result.Success) {
					ModSupport.Log.LogDebug(request.downloadHandler.text);
					lastReportTime = DateTime.Now;
					ModSupportMenus.ShowReportSentMsgBox(callbackAfterReport);
				} else if (responseCode == 429) { // Too Many Requests
					ModSupport.Log.LogError("Request rejected because server is overloaded");
					ModSupportMenus.ShowReportFailServerOverloadedMsgBox(callbackAfterReport);
				} else if (responseCode == 500) { // Internal server error
					ModSupport.Log.LogError("Server error");
					ModSupportMenus.ShowReportFailServerInternalErrorMsgBox(callbackAfterReport);
				} else if (result == UnityWebRequest.Result.ProtocolError || result == UnityWebRequest.Result.ConnectionError) {
					ModSupport.Log.LogError(request.error);
					ModSupportMenus.ShowReportFailConnectionErrorMsgBox(callbackAfterReport);
				} else {
					callbackAfterReport?.Invoke();
				}
			}
		}
	}
}