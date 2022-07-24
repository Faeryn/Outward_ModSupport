using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using UnityEngine.Networking;

namespace ModSupport.Report {
	public class ReportManager {
		private const string ModSupportURL = "http://modsupport.net";
		
		private Report GenerateReport() {
			List<ReportMod> mods = new List<ReportMod>();
			ModList modList = ModList.FromPluginInfos();
			foreach (ModInfo modInfo in modList) {
				mods.Add(new ReportMod(modInfo.GUID, modInfo.Name, modInfo.Version.Major, modInfo.Version.Minor, modInfo.Version.Revision));
			}
			return new Report("Test note", mods, ModSupport.LogHandler.GetCopyOfLogEntries());
		}

		private byte[] ReportToJson(Report report) {
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Report));
			using (MemoryStream memoryStream = new MemoryStream()) {
				serializer.WriteObject(memoryStream, report);
				return memoryStream.ToArray();
			}
		}

		public void SendReport() {
			MenuManager.Instance.ShowConnectionScreen("Sending report, please wait...");
			Report report = GenerateReport();
			ModSupport.Instance.StartCoroutine(SendReportSync(report));
		}

		private IEnumerator SendReportSync(Report report ) {
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
					MenuManager.Instance.ShowMessageBoxOk(null, "Report sent!", 10);
				} else if (responseCode == 429) { // Too Many Requests
					ModSupport.Log.LogError("Request rejected because server is overloaded");
					MenuManager.Instance.ShowMessageBoxOk(null, "Failed to send report because the server is overloaded at the moment. Please try again later!", 10);
				} else if (responseCode == 500) { // Internal server error
					ModSupport.Log.LogError("Server error");
					MenuManager.Instance.ShowMessageBoxOk(null, "Failed to send report because of an internal server error.", 10);
				} else if (result == UnityWebRequest.Result.ProtocolError || result == UnityWebRequest.Result.ConnectionError){
					ModSupport.Log.LogError(request.error);
					MenuManager.Instance.ShowMessageBoxOk(null, "Failed to send report due to connection error.", 10);
				}
			}
		}
	}
}