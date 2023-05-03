using System.Collections.Generic;

namespace Cryville.Common.Buffers {
	/// <summary>
	/// A resource pool that allows reusing instances of lists of type <typeparamref name="T" />.
	/// </summary>
	/// <typeparam name="T">The item type of the lists in the pool.</typeparam>
	public class ListPool<T> {
		private class Bucket : ObjectPool<List<T>> {
			readonly int _size;
			public Bucket(int size, int capacity) : base(capacity) {
				_size = size;
			}
			protected override List<T> Construct() {
				return new List<T>(_size);
			}
		}
		readonly Bucket[] _buckets;
		/// <summary>
		/// Creates an instance of the <see cref="ListPool{T}" /> class with the default maximum list size and bucket capacity.
		/// </summary>
		public ListPool() : this(0x40000000, 256) { }
		/// <summary>
		/// Creates an instance of the <see cref="ListPool{T}" /> class.
		/// </summary>
		/// <param name="maxSize">The maximum size of the lists in the pool.</param>
		/// <param name="capacityPerBucket">The capacity of each bucket. The pool groups lists of similar sizes into buckets for faster access.</param>
		public ListPool(int maxSize, int capacityPerBucket) {
			if (maxSize < 16) maxSize = 16;
			int num = GetID(maxSize) + 1;
			_buckets = new Bucket[num];
			for (int i = 0; i < num; i++) {
				_buckets[i] = new Bucket(GetSize(i), capacityPerBucket);
			}
		}
		/// <summary>
		/// Rents a list of the specified size from the pool. The size of the list must not be changed when it is rented.
		/// </summary>
		/// <param name="size">The size of the list.</param>
		/// <returns>A <see cref="List{T}" /> of the specified size.</returns>
		public List<T> Rent(int size) {
			int len2 = size;
			if (len2 < 16) len2 = 16;
			var list = _buckets[GetID(len2)].Rent();
			if (list.Count < size)
				for (int i = list.Count; i < size; i++) list.Add(default);
			else if (list.Count > size)
				list.RemoveRange(size, list.Count - size);
			return list;
		}
		/// <summary>
		/// Returns a rented list to the pool.
		/// </summary>
		/// <param name="list">The list to return.</param>
		public void Return(List<T> list) {
			int len2 = list.Capacity;
			if (len2 < 16) len2 = 16;
			_buckets[GetID(len2)].Return(list);
		}
		static int GetID(int size) {
			size -= 1;
			size >>= 4;
			int num = 0;
			for (; size != 0; size >>= 1) num++;
			return num;
		}
		static int GetSize(int id) {
			return 0x10 << id;
		}
	}
}
