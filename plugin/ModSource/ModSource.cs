using System.Collections.Generic;

namespace ModSupport.ModSource {
	public interface IModSource {
		IEnumerable<LocalMod> GetMods();
	}
}