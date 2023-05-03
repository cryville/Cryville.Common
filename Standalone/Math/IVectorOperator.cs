namespace Cryville.Common.Math {
	/// <summary>
	/// Provides a set of operators for vector type <typeparamref name="T" />.
	/// </summary>
	/// <typeparam name="T">The vector type.</typeparam>
	public interface IVectorOperator<T> {
		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="lhs">Lefthand vector.</param>
		/// <param name="rhs">Righthand vector.</param>
		/// <returns>The sum of the two vectors.</returns>
		T Add(T lhs, T rhs);
		/// <summary>
		/// Multiplies a vector with a number.
		/// </summary>
		/// <param name="lhs">The number.</param>
		/// <param name="rhs">The vector.</param>
		/// <returns>The product of the number and the vector.</returns>
		T ScalarMultiply(float lhs, T rhs);
	}
}
