namespace Cryville.Common.Buffers {
	/// <summary>
	/// A resource pool that allows reusing instances of type <typeparamref name="T" />, which has a parameterless constructor.
	/// </summary>
	/// <typeparam name="T">The type of the objects in the pool.</typeparam>
	public class SimpleObjectPool<T> : ObjectPool<T> where T : class, new() {
		/// <summary>
		/// Creates an instance of the <see cref="SimpleObjectPool{T}" /> class.
		/// </summary>
		/// <param name="capacity">The capacity of the pool.</param>
		public SimpleObjectPool(int capacity) : base(capacity) { }
		protected override T Construct() {
			return new T();
		}
	}
}
