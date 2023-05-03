using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cryville.Common.Collections.Generic {
	[DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(PairCollectionDebugView<,>))]
	public struct PairCollection<TKey, TValue> : IDisposable {
		public void Dispose() { }
		readonly IPairList<TKey, TValue> _pairList;
		readonly IDictionary<TKey, TValue> _dictionary;
		public PairCollection(object collection) : this() {
			var type = collection.GetType();
			if (typeof(IPairList<TKey, TValue>).IsAssignableFrom(type)) _pairList = (IPairList<TKey, TValue>)collection;
			else if (typeof(IDictionary<TKey, TValue>).IsAssignableFrom(type)) _dictionary = (IDictionary<TKey, TValue>)collection;
			else throw new ArgumentException("Parameter is not a pair collection");
		}
		public int Count {
			get {
				if (_pairList != null) return _pairList.Count;
				else return _dictionary.Count;
			}
		}
		public void Add(TKey key, TValue value) {
			if (_pairList != null) _pairList.Add(key, value);
			else _dictionary.Add(key, value);
		}
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index) {
			if (_pairList != null) _pairList.CopyTo(array, index);
			else _dictionary.CopyTo(array, index);
		}

		public static bool IsPairCollection(Type type) {
			return typeof(IPairList<TKey, TValue>).IsAssignableFrom(type) || typeof(IDictionary<TKey, TValue>).IsAssignableFrom(type);
		}
	}
	internal class PairCollectionDebugView<TKey, TValue> {
		readonly PairCollection<TKey, TValue> _self;
		public PairCollectionDebugView(PairCollection<TKey, TValue> self) {
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
