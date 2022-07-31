namespace ModSupport.AppLog {
	public class RemoveSteamID : LogProcessor {
		private static readonly string Start = "Looking for ";
		private static readonly string End = "'s saves";
		public string ProcessLog(string log) {
			//TODO Better SteamID matcher (though this possibly matches GoG or whatever other ID)
			if (log.StartsWith(Start) && log.EndsWith(End)) {
				return Start+"[ID REMOVED]"+End;
			}
			return log;
		}
	}
}