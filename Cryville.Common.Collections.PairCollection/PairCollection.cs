using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cryville.Common.Collections {
	[DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(PairCollectionDebugView))]
	public struct PairCollection : IDisposable {
		public void Dispose() { }
		readonly IPairList _pairList;
		readonly IDictionary _dictionary;
		public PairCollection(object collection) : this() {
			var type = collection.GetType();
			if (typeof(IPairList).IsAssignableFrom(type)) _pairList = (IPairList)collection;
			else if (typeof(IDictionary).IsAssignableFrom(type)) _dictionary = (IDictionary)collection;
			else throw new ArgumentException("Parameter is not a pair collection");
		}
		public int Count {
			get {
				if (_pairList != null) return _pairList.Count;
				else return _dictionary.Count;
			}
		}
		public void Add(object key, object value) {
			if (_pairList != null) _pairList.Add(key, value);
			else _dictionary.Add(key, value);
		}
		public void CopyTo(KeyValuePair<object, object>[] array, int index) {
			if (_pairList != null) _pairList.CopyTo(array, index);
			else _dictionary.CopyTo(array, index);
		}

		public static bool IsPairCollection(Type type) {
			return typeof(IPairList).IsAssignableFrom(type) || typeof(IDictionary).IsAssignableFrom(type);
		}
	}
	internal class PairCollectionDebugView {
		readonly PairCollection _self;
		public PairCollectionDebugView(PairCollection self) {
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
