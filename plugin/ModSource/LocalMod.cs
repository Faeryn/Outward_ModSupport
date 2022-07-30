namespace ModSupport.ModSource {
	public class LocalMod {
		public ModInfo ModInfo { get; private set; }
		public string DllName { get; private set; }
		public string FolderName { get; private set; }

		public LocalMod(ModInfo modInfo, string dllName, string folderName) {
			ModInfo = modInfo;
			DllName = dllName;
			FolderName = folderName;
		}

		public override string ToString() {
			return $"LocalMod{{{nameof(ModInfo)}: {ModInfo}, {nameof(DllName)}: {DllName}, {nameof(FolderName)}: {FolderName}}}";
		}
	}
}