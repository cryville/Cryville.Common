namespace Cryville.Common.Buffers {
	/// <summary>
	/// A resource pool that allows reusing instances of type <typeparamref name="T" />.
	/// </summary>
	/// <typeparam name="T">The type of the objects in the pool.</typeparam>
	public abstract class ObjectPool<T> where T : class {
		int _index;
		readonly T[] _objs;
		/// <summary>
		/// Creates an instance of the <see cref="ObjectPool{T}" /> class.
		/// </summary>
		/// <param name="capacity">The capacity of the pool.</param>
		public ObjectPool(int capacity) {
			_objs = new T[capacity];
		}
		/// <summary>
		/// The count of objects rented from the pool.
		/// </summary>
		public int RentedCount { get { return _index; } }
		/// <summary>
		/// Rents a object from the pool.
		/// </summary>
		/// <returns>The rented object.</returns>
		public T Rent() {
			T obj = null;
			if (_index < _objs.Length) {
				obj = _objs[_index];
				_objs[_index++] = null;
			}
			if (obj == null) obj = Construct();
			return obj;
		}
		/// <summary>
		/// Returns a rented object to the pool.
		/// </summary>
		/// <param name="obj">The object to return.</param>
		public void Return(T obj) {
			if (_index > 0) {
				Reset(obj);
				_objs[--_index] = obj;
			}
		}
		/// <summary>
		/// Constructs a new instance of type <typeparamref name="T" />.
		/// </summary>
		/// <returns>The new instance.</returns>
		protected abstract T Construct();
		/// <summary>
		/// Resets an object.
		/// </summary>
		/// <param name="obj">The object.</param>
		protected virtual void Reset(T obj) { }
	}
}
