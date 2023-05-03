using System.Collections.Generic;
using System.Diagnostics;

namespace Cryville.Common.Collections {
	[DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(PairListDebugView))]
	public class PairList : List<KeyValuePair<object, object>>, IPairList {
		public void Add(object key, object value) {
			Add(new KeyValuePair<object, object>(key, value));
		}

		public void Insert(int index, object key, object value) {
			Insert(index, new KeyValuePair<object, object>(key, value));
		}
	}
	internal class PairListDebugView {
		readonly PairList _self;
		public PairListDebugView(PairList self) {
			_self = self;
		}
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public KeyValuePair<object, object>[] Items {
			get {
				KeyValuePair<object, object>[] array = new KeyValuePair<object, object>[_self.Count];
				_self.CopyTo(array, 0);
				return array;
			}
		}
	}
}
