using System.Collections.Generic;

namespace Cryville.Common.Buffers {
	/// <summary>
	/// A set of resource pools categorized by a category type.
	/// </summary>
	/// <typeparam name="TCategory">The category type.</typeparam>
	/// <typeparam name="TObject">The type of the objects in the pool.</typeparam>
	public abstract class CategorizedPool<TCategory, TObject> where TObject : class {
		/// <summary>
		/// The set of underlying pools.
		/// </summary>
		/// <remarks>
		/// <para>The <see cref="Rent(TCategory)" /> and <see cref="Return(TCategory, TObject)" /> method select an underlying pool directly from this set with the category as the key. When overridden, this set must be available since construction.</para>
		/// </remarks>
		protected abstract IDictionary<TCategory, ObjectPool<TObject>> Buckets { get; }
		/// <summary>
		/// The count of objects rented from the set of pools.
		/// </summary>
		public int RentedCount { get; private set; }
		/// <summary>
		/// Rents an object from the pool.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <returns>The rented object.</returns>
		public TObject Rent(TCategory category) {
			var obj = Buckets[category].Rent();
			RentedCount++;
			return obj;
		}
		/// <summary>
		/// Returns a rented object to the pool.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <param name="obj">The object to return.</param>
		public void Return(TCategory category, TObject obj) {
			Buckets[category].Return(obj);
			--RentedCount;
		}
	}
	/// <summary>
	/// A utility to access a categorized pool, representing a single unit that uses a shared categorized pool.
	/// </summary>
	/// <typeparam name="TCategory">The category type.</typeparam>
	/// <typeparam name="TObject">The type of the objects in the pool.</typeparam>
	/// <param name="pool">The categorized pool.</param>
	public class CategorizedPoolAccessor<TCategory, TObject>(CategorizedPool<TCategory, TObject> pool) where TObject : class {
		static readonly SimpleObjectPool<Dictionary<TObject, TCategory>> _dictPool = new(1024);
		Dictionary<TObject, TCategory>? _rented;

		/// <summary>
		/// Rents an object from the pool.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <returns>The rented object.</returns>
		public TObject Rent(TCategory category) {
			var obj = pool.Rent(category);
			_rented ??= _dictPool.Rent();
			_rented.Add(obj, category);
			return obj;
		}
		/// <summary>
		/// Returns a rented object to the pool.
		/// </summary>
		/// <param name="obj">The object to return.</param>
		public void Return(TObject obj) {
			if (_rented == null) return;
			pool.Return(_rented[obj], obj);
			_rented.Remove(obj);
		}
		/// <summary>
		/// Returns all objects rented by this accessor to the pool.
		/// </summary>
		public void ReturnAll() {
			if (_rented == null) return;
			foreach (var obj in _rented) {
				pool.Return(obj.Value, obj.Key);
			}
			_rented.Clear();
			_dictPool.Return(_rented);
			_rented = null;
		}
	}
}
