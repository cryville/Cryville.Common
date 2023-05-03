using System.Collections.Generic;

namespace Cryville.Common.Collections.Generic {
	public interface IPairList<TKey, TValue> : IList<KeyValuePair<TKey, TValue>> {
		void Add(TKey key, TValue value);
		void Insert(int index, TKey key, TValue value);
	}
}
