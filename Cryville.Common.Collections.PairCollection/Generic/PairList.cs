using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cryville.Common.Collections.Generic {
	[DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(PairListDebugView<,>))]
	public class PairList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>, IPairList<TKey, TValue>, IPairList {
		public void Add(TKey key, TValue value) {
			Add(new KeyValuePair<TKey, TValue>(key, value));
		}

		public void Add(object key, object value) {
			try {
				Add((TKey)key, (TValue)value);
			}
			catch (InvalidCastException) {
				throw new ArgumentException("Wrong key type or value type");
			}
		}

		public void Insert(int index, TKey key, TValue value) {
			Insert(index, new KeyValuePair<TKey, TValue>(key, value));
		}

		public void Insert(int index, object key, object value) {
			try {
				Insert(index, (TKey)key, (TValue)value);
			}
			catch (InvalidCastException) {
				throw new ArgumentException("Wrong key type or value type");
			}
		}
	}
	internal class PairListDebugView<TKey, TValue> {
		readonly PairList<TKey, TValue> _self;
		public PairListDebugView(PairList<TKey, TValue> self) {
			_self = self;
		}
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public KeyValuePair<TKey, TValue>[] Items {
			get {
				KeyValuePair<TKey, TValue>[] array = new KeyValuePair<TKey, TValue>[_self.Count];
				_self.CopyTo(array, 0);
				return array;
			}
		}
	}
}
