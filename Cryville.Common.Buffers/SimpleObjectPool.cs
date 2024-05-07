namespace Cryville.Common.Buffers {
	/// <summary>
	/// A resource pool that allows reusing instances of type <typeparamref name="T" />, which has a parameterless constructor.
	/// </summary>
	/// <typeparam name="T">The type of the objects in the pool.</typeparam>
	/// <param name="capacity">The capacity of the pool.</param>
	public class SimpleObjectPool<T>(int capacity) : ObjectPool<T>(capacity) where T : class, new() {
		/// <inheritdoc />
		protected override T Construct() => new();
	}
}
