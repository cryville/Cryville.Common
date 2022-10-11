namespace Cryville.Common.Math {
	/// <summary>
	/// Represents a column vector of vector type <typeparamref name="T" />.
	/// </summary>
	/// <typeparam name="T">The vector type of the elements.</typeparam>
	public class ColumnVector<T> {
		readonly T[] content;
		/// <summary>
		/// The size of the vector.
		/// </summary>
		public int Size {
			get;
			private set;
		}
		/// <summary>
		/// Creates a column vector with specified size.
		/// </summary>
		/// <param name="size">The size of the vector.</param>
		public ColumnVector(int size) {
			content = new T[size];
			Size = size;
		}
		/// <summary>
		/// Creates a column vector from an array.
		/// </summary>
		/// <param name="c">The array.</param>
		public ColumnVector(T[] c) {
			Size = c.Length;
			content = c;
		}
		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="i">The zero-based index of the element to get or set.</param>
		/// <returns>The element at the specified index.</returns>
		public T this[int i] {
			get {
				return content[i];
			}
			set {
				content[i] = value;
			}
		}
		/// <summary>
		/// Performs dot operation with a <see cref="System.Single" /> column vector.
		/// </summary>
		/// <param name="lhs">The lefthand column vector.</param>
		/// <param name="o">The vector operator.</param>
		/// <returns>The result of the dot operation.</returns>
		public T Dot(ColumnVector<float> lhs, IVectorOperator<T> o) {
			T res = default(T);
			for (var i = 0; i < Size; i++)
				res = o.Add(res, o.ScalarMultiply(lhs[i], content[i]));
			return res;
		}
		/// <summary>
		/// Creates a <see cref="System.Single" /> column vector and fills it with polynomial coefficients.
		/// </summary>
		/// <param name="size">The size of the column vector.</param>
		/// <param name="num">The base number.</param>
		/// <returns>A <see cref="System.Single" /> column vector filled with polynomial coefficients.</returns>
		public static ColumnVector<float> WithPolynomialCoefficients(int size, float num) {
			var m = new ColumnVector<float>(size);
			for (var i = 0; i < size; i++)
				m[i] = (float)System.Math.Pow(num, i);
			return m;
		}
	}
}
