namespace Cryville.Common.Buffers {
	/// <summary>
	/// A resource pool that allows reusing instances of arrays of type <typeparamref name="T" />.
	/// </summary>
	/// <typeparam name="T">The item type of the arrays in the pool.</typeparam>
	public class ArrayPool<T> {
		sealed class Bucket(int size, int capacity) : ObjectPool<T[]>(capacity) {
			protected override T[] Construct() => new T[size];
		}
		readonly Bucket[] _buckets;
		/// <summary>
		/// Creates an instance of the <see cref="ArrayPool{T}" /> class with the default maximum list size and bucket capacity.
		/// </summary>
		public ArrayPool() : this(0x40000000, 256) { }
		/// <summary>
		/// Creates an instance of the <see cref="ArrayPool{T}" /> class.
		/// </summary>
		/// <param name="maxSize">The maximum size of the arrays in the pool.</param>
		/// <param name="capacityPerBucket">The capacity of each bucket. The pool groups arrays of similar sizes into buckets for faster access.</param>
		public ArrayPool(int maxSize, int capacityPerBucket) {
			if (maxSize < 16) maxSize = 16;
			int num = GetID(maxSize) + 1;
			_buckets = new Bucket[num];
			for (int i = 0; i < num; i++) {
				_buckets[i] = new Bucket(GetSize(i), capacityPerBucket);
			}
		}
		/// <summary>
		/// Rents an array that is at least the specified size from the pool.
		/// </summary>
		/// <param name="size">The minimum size of the array.</param>
		/// <returns>An array of type <typeparamref name="T" /> that is at least the specified size.</returns>
		public T[] Rent(int size) {
			int len2 = size;
			if (len2 < 16) len2 = 16;
			var arr = _buckets[GetID(len2)].Rent();
			return arr;
		}
		/// <summary>
		/// Returns a rented array to the pool.
		/// </summary>
		/// <param name="arr">The array to return.</param>
		public void Return(T[] arr) {
			int len2 = arr.Length;
			if (len2 < 16) len2 = 16;
			_buckets[GetID(len2)].Return(arr);
		}
		static int GetID(int size) {
			size -= 1;
			size >>= 4;
			int num = 0;
			for (; size != 0; size >>= 1) num++;
			return num;
		}
		static int GetSize(int id) => 0x10 << id;
	}
}
