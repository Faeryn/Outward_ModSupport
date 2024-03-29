using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModSupport {
	public class ModList : IEnumerable<ModInfo> {
		public static readonly ModList Empty = new ModList();
		
		private List<ModInfo> list = new List<ModInfo>();

		protected ModList() {
		}

		public ModList(IEnumerable<ModInfo> modInfos) {
			foreach (ModInfo info in modInfos) {
				list.Add(info);
			}
			list.Sort((modA, modB) => String.Compare(modA.Name, modB.Name, StringComparison.OrdinalIgnoreCase));
		}
		
		public ModInfo FindByGuid(string guid) {
			return list.Find(mod => mod.GUID == guid);
		}

		public IEnumerator<ModInfo> GetEnumerator() {
			return list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public bool isEmpty() {
			return list.Count == 0;
		}

		public override string ToString() {
			return list.Aggregate(
					new StringBuilder(), 
					(current, next) => current.Append(current.Length == 0? "" : ", ").Append(next))
				.ToString();
		}
		
		/**
		 * Used for Photon serialization
		 */
		public ExitGames.Client.Photon.Hashtable[] ToDictArray() {
			ExitGames.Client.Photon.Hashtable[] dictArray = new ExitGames.Client.Photon.Hashtable[list.Count];
			int index = 0;
			foreach (ModInfo mod in list) {
				dictArray[index]=mod.ToDict();
				index++;
			}
			return dictArray;
		}

		/**
		 * Used for Photon deserialization
		 */
		public static ModList FromDictArray(ExitGames.Client.Photon.Hashtable[] dictArray) {
			if (dictArray == null) {
				return null;
			}
			List<ModInfo> modInfos = new List<ModInfo>();
			foreach (ExitGames.Client.Photon.Hashtable dict in dictArray) {
				modInfos.Add(ModInfo.FromDict(dict));
			}
			return new ModList(modInfos);
		}
	}
}