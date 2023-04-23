using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Cryville.Common.Collections.Specialized {
	/// <summary>
	/// Represents a collection of <see cref="int" /> keys and values. Identical to <see cref="Dictionary{int, T}" /> but much faster.
	/// </summary>
	/// <typeparam name="T">The type of the values in the dictionary.</typeparam>
	[DebuggerTypeProxy(typeof(IntKeyedDictionaryDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	public class IntKeyedDictionary<T> : IDictionary<int, T>, IDictionary, IReadOnlyDictionary<int, T> {

		private struct Entry {
			public int next;        // Index of next entry, -1 if last
			public int key;           // Key of entry
			public T value;         // Value of entry
		}

		private int[] buckets;
		private Entry[] entries;
		private int count;
		private int version;
		private int freeList;
		private int freeCount;
		private KeyCollection keys;
		private ValueCollection values;
		private Object _syncRoot;

		public IntKeyedDictionary() : this(0) { }

		public IntKeyedDictionary(int capacity) {
			if (capacity < 0) throw new ArgumentOutOfRangeException("capacity");
			if (capacity > 0) Initialize(capacity);
		}

		public IntKeyedDictionary(IDictionary<int, T> dictionary) :
			this(dictionary != null ? dictionary.Count : 0) {

			if (dictionary == null) {
				throw new ArgumentNullException("dictionary");
			}

			foreach (KeyValuePair<int, T> pair in dictionary) {
				Add(pair.Key, pair.Value);
			}
		}

		public int Count {
			get { return count - freeCount; }
		}

		public KeyCollection Keys {
			get {
				Contract.Ensures(Contract.Result<KeyCollection>() != null);
				if (keys == null) keys = new KeyCollection(this);
				return keys;
			}
		}

		ICollection<int> IDictionary<int, T>.Keys {
			get {
				if (keys == null) keys = new KeyCollection(this);
				return keys;
			}
		}

		IEnumerable<int> IReadOnlyDictionary<int, T>.Keys {
			get {
				if (keys == null) keys = new KeyCollection(this);
				return keys;
			}
		}

		public ValueCollection Values {
			get {
				Contract.Ensures(Contract.Result<ValueCollection>() != null);
				if (values == null) values = new ValueCollection(this);
				return values;
			}
		}

		ICollection<T> IDictionary<int, T>.Values {
			get {
				if (values == null) values = new ValueCollection(this);
				return values;
			}
		}

		IEnumerable<T> IReadOnlyDictionary<int, T>.Values {
			get {
				if (values == null) values = new ValueCollection(this);
				return values;
			}
		}

		public T this[int key] {
			get {
				int i = FindEntry(key);
				if (i >= 0) return entries[i].value;
				throw new KeyNotFoundException();
			}
			set {
				Insert(key, value, false);
			}
		}

		public void Add(int key, T value) {
			Insert(key, value, true);
		}

		void ICollection<KeyValuePair<int, T>>.Add(KeyValuePair<int, T> keyValuePair) {
			Add(keyValuePair.Key, keyValuePair.Value);
		}

		bool ICollection<KeyValuePair<int, T>>.Contains(KeyValuePair<int, T> keyValuePair) {
			int i = FindEntry(keyValuePair.Key);
			if (i >= 0) {
				return true;
			}
			return false;
		}

		bool ICollection<KeyValuePair<int, T>>.Remove(KeyValuePair<int, T> keyValuePair) {
			int i = FindEntry(keyValuePair.Key);
			if (i >= 0) {
				Remove(keyValuePair.Key);
				return true;
			}
			return false;
		}

		public void Clear() {
			if (count > 0) {
				for (int i = 0; i < buckets.Length; i++) buckets[i] = -1;
				Array.Clear(entries, 0, count);
				freeList = -1;
				count = 0;
				freeCount = 0;
				version++;
			}
		}

		public bool ContainsKey(int key) {
			return FindEntry(key) >= 0;
		}

		public bool ContainsValue(T value) {
			if (value == null) {
				for (int i = 0; i < count; i++) {
					if (entries[i].key >= 0 && entries[i].value == null) return true;
				}
			}
			else {
				EqualityComparer<T> c = EqualityComparer<T>.Default;
				for (int i = 0; i < count; i++) {
					if (entries[i].key >= 0 && c.Equals(entries[i].value, value)) return true;
				}
			}
			return false;
		}

		private void CopyTo(KeyValuePair<int, T>[] array, int index) {
			if (array == null) {
				throw new ArgumentNullException("array");
			}

			if (index < 0 || index > array.Length) {
				throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
			}

			if (array.Length - index < Count) {
				throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
			}

			int count = this.count;
			Entry[] entries = this.entries;
			for (int i = 0; i < count; i++) {
				if (entries[i].key >= 0) {
					array[index++] = new KeyValuePair<int, T>(entries[i].key, entries[i].value);
				}
			}
		}

		public Enumerator GetEnumerator() {
			return new Enumerator(this, Enumerator.KeyValuePair);
		}

		IEnumerator<KeyValuePair<int, T>> IEnumerable<KeyValuePair<int, T>>.GetEnumerator() {
			return new Enumerator(this, Enumerator.KeyValuePair);
		}

		private int FindEntry(int key) {
			if (buckets != null) {
				for (int i = buckets[key % buckets.Length]; i >= 0; i = entries[i].next) {
					if (entries[i].key == key) return i;
				}
			}
			return -1;
		}

		private void Initialize(int capacity) {
			int size = HashHelpers.GetPrime(capacity);
			buckets = new int[size];
			for (int i = 0; i < buckets.Length; i++) buckets[i] = -1;
			entries = new Entry[size];
			freeList = -1;
		}

		private void Insert(int key, T value, bool add) {
			if (buckets == null) Initialize(0);
			int targetBucket = key % buckets.Length;

#if FEATURE_RANDOMIZED_STRING_HASHING
            int collisionCount = 0;
#endif

			for (int i = buckets[targetBucket]; i >= 0; i = entries[i].next) {
				if (entries[i].key == key) {
					if (add) {
						throw new ArgumentException("An item with the same key has already been added.");
					}
					entries[i].value = value;
					version++;
					return;
				}

#if FEATURE_RANDOMIZED_STRING_HASHING
                collisionCount++;
#endif
			}
			int index;
			if (freeCount > 0) {
				index = freeList;
				freeList = entries[index].next;
				freeCount--;
			}
			else {
				if (count == entries.Length) {
					Resize();
					targetBucket = key % buckets.Length;
				}
				index = count;
				count++;
			}

			entries[index].next = buckets[targetBucket];
			entries[index].key = key;
			entries[index].value = value;
			buckets[targetBucket] = index;
			version++;
		}

		private void Resize() {
			Resize(HashHelpers.ExpandPrime(count), false);
		}

		private void Resize(int newSize, bool forceNewHashCodes) {
			Contract.Assert(newSize >= entries.Length);
			int[] newBuckets = new int[newSize];
			for (int i = 0; i < newBuckets.Length; i++) newBuckets[i] = -1;
			Entry[] newEntries = new Entry[newSize];
			Array.Copy(entries, 0, newEntries, 0, count);
			if (forceNewHashCodes) {
				for (int i = 0; i < count; i++) {
					if (newEntries[i].key != -1) {
						newEntries[i].key = newEntries[i].key;
					}
				}
			}
			for (int i = 0; i < count; i++) {
				if (newEntries[i].key >= 0) {
					int bucket = newEntries[i].key % newSize;
					newEntries[i].next = newBuckets[bucket];
					newBuckets[bucket] = i;
				}
			}
			buckets = newBuckets;
			entries = newEntries;
		}

		public bool Remove(int key) {
			if (buckets != null) {
				int bucket = key % buckets.Length;
				int last = -1;
				for (int i = buckets[bucket]; i >= 0; last = i, i = entries[i].next) {
					if (entries[i].key == key) {
						if (last < 0) {
							buckets[bucket] = entries[i].next;
						}
						else {
							entries[last].next = entries[i].next;
						}
						entries[i].key = -1;
						entries[i].next = freeList;
						entries[i].value = default(T);
						freeList = i;
						freeCount++;
						version++;
						return true;
					}
				}
			}
			return false;
		}

		public bool TryGetValue(int key, out T value) {
			int i = FindEntry(key);
			if (i >= 0) {
				value = entries[i].value;
				return true;
			}
			value = default(T);
			return false;
		}

		// This is a convenience method for the internal callers that were converted from using Hashtable.
		// Many were combining key doesn't exist and key exists but null value (for non-value types) checks.
		// This allows them to continue getting that behavior with minimal code delta. This is basically
		// TryGetValue without the out param
		internal T GetValueOrDefault(int key) {
			int i = FindEntry(key);
			if (i >= 0) {
				return entries[i].value;
			}
			return default(T);
		}

		bool ICollection<KeyValuePair<int, T>>.IsReadOnly {
			get { return false; }
		}

		void ICollection<KeyValuePair<int, T>>.CopyTo(KeyValuePair<int, T>[] array, int index) {
			CopyTo(array, index);
		}

		void ICollection.CopyTo(Array array, int index) {
			if (array == null) {
				throw new ArgumentNullException("array");
			}

			if (array.Rank != 1) {
				throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
			}

			if (array.GetLowerBound(0) != 0) {
				throw new ArgumentException("The lower bound of target array must be zero.");
			}

			if (index < 0 || index > array.Length) {
				throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
			}

			if (array.Length - index < Count) {
				throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
			}

			KeyValuePair<int,T>[] pairs = array as KeyValuePair<int,T>[];
			if (pairs != null) {
				CopyTo(pairs, index);
			}
			else if (array is DictionaryEntry[]) {
				DictionaryEntry[] dictEntryArray = array as DictionaryEntry[];
				Entry[] entries = this.entries;
				for (int i = 0; i < count; i++) {
					if (entries[i].key >= 0) {
						dictEntryArray[index++] = new DictionaryEntry(entries[i].key, entries[i].value);
					}
				}
			}
			else {
				object[] objects = array as object[];
				if (objects == null) {
					throw new ArgumentException("Target array type is not compatible with the type of items in the collection.");
				}

				try {
					int count = this.count;
					Entry[] entries = this.entries;
					for (int i = 0; i < count; i++) {
						if (entries[i].key >= 0) {
							objects[index++] = new KeyValuePair<int, T>(entries[i].key, entries[i].value);
						}
					}
				}
				catch (ArrayTypeMismatchException) {
					throw new ArgumentException("Target array type is not compatible with the type of items in the collection.");
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return new Enumerator(this, Enumerator.KeyValuePair);
		}

		bool ICollection.IsSynchronized {
			get { return false; }
		}

		object ICollection.SyncRoot {
			get {
				if (_syncRoot == null) {
					System.Threading.Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);
				}
				return _syncRoot;
			}
		}

		bool IDictionary.IsFixedSize {
			get { return false; }
		}

		bool IDictionary.IsReadOnly {
			get { return false; }
		}

		ICollection IDictionary.Keys {
			get { return (ICollection)Keys; }
		}

		ICollection IDictionary.Values {
			get { return (ICollection)Values; }
		}

		object IDictionary.this[object key] {
			get {
				if (IsCompatibleKey(key)) {
					int i = FindEntry((int)key);
					if (i >= 0) {
						return entries[i].value;
					}
				}
				return null;
			}
			set {
				if (key == null) {
					throw new ArgumentNullException("key");
				}
				if (value == null && default(T) != null) {
					throw new ArgumentNullException("value");
				}

				try {
					int tempKey = (int)key;
					try {
						this[tempKey] = (T)value;
					}
					catch (InvalidCastException) {
						throw new ArgumentException(string.Format("The value \"{0}\" is not of type \"{1}\" and cannot be used in this generic collection.", value, typeof(T)), "value");
					}
				}
				catch (InvalidCastException) {
					throw new ArgumentException(string.Format("The value \"{0}\" is not of type \"{1}\" and cannot be used in this generic collection.", key, typeof(int)), "key");
				}
			}
		}

		private static bool IsCompatibleKey(object key) {
			if (key == null) {
				throw new ArgumentNullException("key");
			}
			return (key is int);
		}

		void IDictionary.Add(object key, object value) {
			if (key == null) {
				throw new ArgumentNullException("key");
			}
			if (value == null && default(T) != null) {
				throw new ArgumentNullException("value");
			}

			try {
				int tempKey = (int)key;

				try {
					Add(tempKey, (T)value);
				}
				catch (InvalidCastException) {
					throw new ArgumentException(string.Format("The value \"{0}\" is not of type \"{1}\" and cannot be used in this generic collection.", value, typeof(T)), "value");
				}
			}
			catch (InvalidCastException) {
				throw new ArgumentException(string.Format("The value \"{0}\" is not of type \"{1}\" and cannot be used in this generic collection.", key, typeof(int)), "key");
			}
		}

		bool IDictionary.Contains(object key) {
			if (IsCompatibleKey(key)) {
				return ContainsKey((int)key);
			}

			return false;
		}

		IDictionaryEnumerator IDictionary.GetEnumerator() {
			return new Enumerator(this, Enumerator.DictEntry);
		}

		void IDictionary.Remove(object key) {
			if (IsCompatibleKey(key)) {
				Remove((int)key);
			}
		}

		[Serializable]
		public struct Enumerator : IEnumerator<KeyValuePair<int, T>>,
			IDictionaryEnumerator {
			private readonly IntKeyedDictionary<T> dictionary;
			private readonly int version;
			private int index;
			private KeyValuePair<int,T> current;
			private readonly int getEnumeratorRetType;  // What should Enumerator.Current return?

			internal const int DictEntry = 1;
			internal const int KeyValuePair = 2;

			internal Enumerator(IntKeyedDictionary<T> dictionary, int getEnumeratorRetType) {
				this.dictionary = dictionary;
				version = dictionary.version;
				index = 0;
				this.getEnumeratorRetType = getEnumeratorRetType;
				current = new KeyValuePair<int, T>();
			}

			public bool MoveNext() {
				if (version != dictionary.version) {
					throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
				}

				// Use unsigned comparison since we set index to dictionary.count+1 when the enumeration ends.
				// dictionary.count+1 could be negative if dictionary.count is Int32.MaxValue
				while ((uint)index < (uint)dictionary.count) {
					if (dictionary.entries[index].key >= 0) {
						current = new KeyValuePair<int, T>(dictionary.entries[index].key, dictionary.entries[index].value);
						index++;
						return true;
					}
					index++;
				}

				index = dictionary.count + 1;
				current = new KeyValuePair<int, T>();
				return false;
			}

			public KeyValuePair<int, T> Current {
				get { return current; }
			}

			public void Dispose() {
			}

			object IEnumerator.Current {
				get {
					if (index == 0 || (index == dictionary.count + 1)) {
						throw new InvalidOperationException("Enumeration has either not started or has already finished.");
					}

					if (getEnumeratorRetType == DictEntry) {
						return new System.Collections.DictionaryEntry(current.Key, current.Value);
					}
					else {
						return new KeyValuePair<int, T>(current.Key, current.Value);
					}
				}
			}

			void IEnumerator.Reset() {
				if (version != dictionary.version) {
					throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
				}

				index = 0;
				current = new KeyValuePair<int, T>();
			}

			DictionaryEntry IDictionaryEnumerator.Entry {
				get {
					if (index == 0 || (index == dictionary.count + 1)) {
						throw new InvalidOperationException("Enumeration has either not started or has already finished.");
					}

					return new DictionaryEntry(current.Key, current.Value);
				}
			}

			object IDictionaryEnumerator.Key {
				get {
					if (index == 0 || (index == dictionary.count + 1)) {
						throw new InvalidOperationException("Enumeration has either not started or has already finished.");
					}

					return current.Key;
				}
			}

			object IDictionaryEnumerator.Value {
				get {
					if (index == 0 || (index == dictionary.count + 1)) {
						throw new InvalidOperationException("Enumeration has either not started or has already finished.");
					}

					return current.Value;
				}
			}
		}

		[DebuggerTypeProxy(typeof(IntKeyedDictionaryKeyCollectionDebugView<>))]
		[DebuggerDisplay("Count = {Count}")]
		[Serializable]
		public sealed class KeyCollection : ICollection<int>, ICollection, IReadOnlyCollection<int> {
			private readonly IntKeyedDictionary<T> dictionary;

			public KeyCollection(IntKeyedDictionary<T> dictionary) {
				if (dictionary == null) {
					throw new ArgumentNullException("dictionary");
				}
				this.dictionary = dictionary;
			}

			public Enumerator GetEnumerator() {
				return new Enumerator(dictionary);
			}

			public void CopyTo(int[] array, int index) {
				if (array == null) {
					throw new ArgumentNullException("array");
				}

				if (index < 0 || index > array.Length) {
					throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
				}

				if (array.Length - index < dictionary.Count) {
					throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
				}

				int count = dictionary.count;
				Entry[] entries = dictionary.entries;
				for (int i = 0; i < count; i++) {
					if (entries[i].key >= 0) array[index++] = entries[i].key;
				}
			}

			public int Count {
				get { return dictionary.Count; }
			}

			bool ICollection<int>.IsReadOnly {
				get { return true; }
			}

			void ICollection<int>.Add(int item) {
				throw new NotSupportedException("Mutating a key collection derived from a dictionary is not allowed.");
			}

			void ICollection<int>.Clear() {
				throw new NotSupportedException("Mutating a key collection derived from a dictionary is not allowed.");
			}

			bool ICollection<int>.Contains(int item) {
				return dictionary.ContainsKey(item);
			}

			bool ICollection<int>.Remove(int item) {
				throw new NotSupportedException("Mutating a key collection derived from a dictionary is not allowed.");
			}

			IEnumerator<int> IEnumerable<int>.GetEnumerator() {
				return new Enumerator(dictionary);
			}

			IEnumerator IEnumerable.GetEnumerator() {
				return new Enumerator(dictionary);
			}

			void ICollection.CopyTo(Array array, int index) {
				if (array == null) {
					throw new ArgumentNullException("array");
				}

				if (array.Rank != 1) {
					throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
				}

				if (array.GetLowerBound(0) != 0) {
					throw new ArgumentException("The lower bound of target array must be zero.");
				}

				if (index < 0 || index > array.Length) {
					throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
				}

				if (array.Length - index < dictionary.Count) {
					throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
				}

				int[] keys = array as int[];
				if (keys != null) {
					CopyTo(keys, index);
				}
				else {
					object[] objects = array as object[];
					if (objects == null) {
						throw new ArgumentException("Target array type is not compatible with the type of items in the collection.");
					}

					int count = dictionary.count;
					Entry[] entries = dictionary.entries;
					try {
						for (int i = 0; i < count; i++) {
							if (entries[i].key >= 0) objects[index++] = entries[i].key;
						}
					}
					catch (ArrayTypeMismatchException) {
						throw new ArgumentException("Target array type is not compatible with the type of items in the collection.");
					}
				}
			}

			bool ICollection.IsSynchronized {
				get { return false; }
			}

			Object ICollection.SyncRoot {
				get { return ((ICollection)dictionary).SyncRoot; }
			}

			[Serializable]
			public struct Enumerator : IEnumerator<int>, System.Collections.IEnumerator {
				private readonly IntKeyedDictionary<T> dictionary;
				private int index;
				private readonly int version;
				private int currentKey;

				internal Enumerator(IntKeyedDictionary<T> dictionary) {
					this.dictionary = dictionary;
					version = dictionary.version;
					index = 0;
					currentKey = default(int);
				}

				public void Dispose() {
				}

				public bool MoveNext() {
					if (version != dictionary.version) {
						throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
					}

					while ((uint)index < (uint)dictionary.count) {
						if (dictionary.entries[index].key >= 0) {
							currentKey = dictionary.entries[index].key;
							index++;
							return true;
						}
						index++;
					}

					index = dictionary.count + 1;
					currentKey = default(int);
					return false;
				}

				public int Current {
					get {
						return currentKey;
					}
				}

				Object System.Collections.IEnumerator.Current {
					get {
						if (index == 0 || (index == dictionary.count + 1)) {
							throw new InvalidOperationException("Enumeration has either not started or has already finished.");
						}

						return currentKey;
					}
				}

				void System.Collections.IEnumerator.Reset() {
					if (version != dictionary.version) {
						throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
					}

					index = 0;
					currentKey = default(int);
				}
			}
		}

		[DebuggerTypeProxy(typeof(IntKeyedDictionaryValueCollectionDebugView<>))]
		[DebuggerDisplay("Count = {Count}")]
		[Serializable]
		public sealed class ValueCollection : ICollection<T>, ICollection, IReadOnlyCollection<T> {
			private readonly IntKeyedDictionary<T> dictionary;

			public ValueCollection(IntKeyedDictionary<T> dictionary) {
				if (dictionary == null) {
					throw new ArgumentNullException("dictionary");
				}
				this.dictionary = dictionary;
			}

			public Enumerator GetEnumerator() {
				return new Enumerator(dictionary);
			}

			public void CopyTo(T[] array, int index) {
				if (array == null) {
					throw new ArgumentNullException("array");
				}

				if (index < 0 || index > array.Length) {
					throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
				}

				if (array.Length - index < dictionary.Count) {
					throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
				}

				int count = dictionary.count;
				Entry[] entries = dictionary.entries;
				for (int i = 0; i < count; i++) {
					if (entries[i].key >= 0) array[index++] = entries[i].value;
				}
			}

			public int Count {
				get { return dictionary.Count; }
			}

			bool ICollection<T>.IsReadOnly {
				get { return true; }
			}

			void ICollection<T>.Add(T item) {
				throw new NotSupportedException("Mutating a value collection derived from a dictionary is not allowed.");
			}

			bool ICollection<T>.Remove(T item) {
				throw new NotSupportedException("Mutating a value collection derived from a dictionary is not allowed.");
			}

			void ICollection<T>.Clear() {
				throw new NotSupportedException("Mutating a value collection derived from a dictionary is not allowed.");
			}

			bool ICollection<T>.Contains(T item) {
				return dictionary.ContainsValue(item);
			}

			IEnumerator<T> IEnumerable<T>.GetEnumerator() {
				return new Enumerator(dictionary);
			}

			IEnumerator IEnumerable.GetEnumerator() {
				return new Enumerator(dictionary);
			}

			void ICollection.CopyTo(Array array, int index) {
				if (array == null) {
					throw new ArgumentNullException("array");
				}

				if (array.Rank != 1) {
					throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
				}

				if (array.GetLowerBound(0) != 0) {
					throw new ArgumentException("The lower bound of target array must be zero.");
				}

				if (index < 0 || index > array.Length) {
					throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
				}

				if (array.Length - index < dictionary.Count) {
					throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
				}

				T[] values = array as T[];
				if (values != null) {
					CopyTo(values, index);
				}
				else {
					object[] objects = array as object[];
					if (objects == null) {
						throw new ArgumentException("Target array type is not compatible with the type of items in the collection.");
					}

					int count = dictionary.count;
					Entry[] entries = dictionary.entries;
					try {
						for (int i = 0; i < count; i++) {
							if (entries[i].key >= 0) objects[index++] = entries[i].value;
						}
					}
					catch (ArrayTypeMismatchException) {
						throw new ArgumentException("Target array type is not compatible with the type of items in the collection.");
					}
				}
			}

			bool ICollection.IsSynchronized {
				get { return false; }
			}

			Object ICollection.SyncRoot {
				get { return ((ICollection)dictionary).SyncRoot; }
			}

			[Serializable]
			public struct Enumerator : IEnumerator<T>, IEnumerator {
				private readonly IntKeyedDictionary<T> dictionary;
				private int index;
				private readonly int version;
				private T currentValue;

				internal Enumerator(IntKeyedDictionary<T> dictionary) {
					this.dictionary = dictionary;
					version = dictionary.version;
					index = 0;
					currentValue = default(T);
				}

				public void Dispose() {
				}

				public bool MoveNext() {
					if (version != dictionary.version) {
						throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
					}

					while ((uint)index < (uint)dictionary.count) {
						if (dictionary.entries[index].key >= 0) {
							currentValue = dictionary.entries[index].value;
							index++;
							return true;
						}
						index++;
					}
					index = dictionary.count + 1;
					currentValue = default(T);
					return false;
				}

				public T Current {
					get {
						return currentValue;
					}
				}

				Object System.Collections.IEnumerator.Current {
					get {
						if (index == 0 || (index == dictionary.count + 1)) {
							throw new InvalidOperationException("Enumeration has either not started or has already finished.");
						}

						return currentValue;
					}
				}

				void System.Collections.IEnumerator.Reset() {
					if (version != dictionary.version) {
						throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
					}
					index = 0;
					currentValue = default(T);
				}
			}
		}
	}

	internal sealed class IntKeyedDictionaryDebugView<T> {
		private readonly IntKeyedDictionary<T> dict;

		public IntKeyedDictionaryDebugView(IntKeyedDictionary<T> dictionary) {
			if (dictionary == null)
				throw new ArgumentNullException("dictionary");

			this.dict = dictionary;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public KeyValuePair<int, T>[] Items {
			get {
				KeyValuePair<int, T>[] items = new KeyValuePair<int, T>[dict.Count];
				((IDictionary)dict).CopyTo(items, 0);
				return items;
			}
		}
	}

	internal sealed class IntKeyedDictionaryKeyCollectionDebugView<T> {
		private readonly IntKeyedDictionary<T> collection;

		public IntKeyedDictionaryKeyCollectionDebugView(IntKeyedDictionary<T> collection) {
			if (collection == null)
				throw new ArgumentNullException("collection");

			this.collection = collection;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items {
			get {
				T[] items = new T[collection.Count];
				((IDictionary)collection).CopyTo(items, 0);
				return items;
			}
		}
	}

	internal sealed class IntKeyedDictionaryValueCollectionDebugView<T> {
		private readonly IntKeyedDictionary<T> collection;

		public IntKeyedDictionaryValueCollectionDebugView(IntKeyedDictionary<T> collection) {
			if (collection == null)
				throw new ArgumentNullException("collection");

			this.collection = collection;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items {
			get {
				T[] items = new T[collection.Count];
				((IDictionary)collection).CopyTo(items, 0);
				return items;
			}
		}
	}
}
