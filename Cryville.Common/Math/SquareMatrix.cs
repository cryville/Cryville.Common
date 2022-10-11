namespace Cryville.Common.Math {
	/// <summary>
	/// Represents a square matrix.
	/// </summary>
	public class SquareMatrix {
		readonly float[,] content;
		/// <summary>
		/// The size of the matrix.
		/// </summary>
		public int Size {
			get;
			private set;
		}
		/// <summary>
		/// Creates a square matrix with the specified size.
		/// </summary>
		/// <param name="size">The size of the matrix.</param>
		public SquareMatrix(int size) {
			content = new float[size, size];
			Size = size;
		}
		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="r">The zero-based row index.</param>
		/// <param name="c">The zero-based column index.</param>
		/// <returns>The element at the specified index.</returns>
		public float this[int r, int c] {
			get { return content[r, c]; }
			set { content[r, c] = value; }
		}
		/// <summary>
		/// Eliminates the square matrix against a column vector.
		/// </summary>
		/// <typeparam name="T">The vector type.</typeparam>
		/// <param name="v">The column vector.</param>
		/// <param name="o">The column operator.</param>
		/// <returns>The column vector eliminated.</returns>
		public ColumnVector<T> Eliminate<T>(ColumnVector<T> v, IVectorOperator<T> o) {
			int s = Size;
			float[,] d = (float[,])content.Clone();
			int[] refl = new int[s];
			for (int i = 0; i < s; i++)
				refl[i] = i;
			for (int r = 0; r < s; r++) {
				for (int r0 = r; r0 < s; r0++)
					if (d[refl[r0], r] != 0) {
						refl[r] = r0;
						refl[r0] = r;
						break;
					}
				int or = refl[r];
				float sf0 = d[or, r];
				for (int c0 = r; c0 < s; c0++)
					d[or, c0] /= sf0;
				v[or] = o.ScalarMultiply(1 / sf0, v[or]);
				for (int r1 = r + 1; r1 < s; r1++) {
					int or1 = refl[r1];
					float sf1 = d[or1, r];
					for (int c1 = r; c1 < s; c1++)
						d[or1, c1] -= d[or, c1] * sf1;
					v[or1] = o.Add(v[or1], o.ScalarMultiply(-sf1, v[or]));
				}
			}
			T[] res = new T[s];
			for (int r2 = s - 1; r2 >= 0; r2--) {
				var v2 = v[refl[r2]];
				for (int c2 = r2 + 1; c2 < s; c2++)
					v2 = o.Add(v2, o.ScalarMultiply(-d[refl[r2], c2], res[refl[c2]]));
				res[refl[r2]] = v2;
			}
			return new ColumnVector<T>(res);
		}
		/// <summary>
		/// Creates a square matrix and fills it with polynomial coefficients.
		/// </summary>
		/// <param name="size">The size of the square matrix.</param>
		/// <returns>A square matrix filled with polynomial coefficients.</returns>
		public static SquareMatrix WithPolynomialCoefficients(int size) {
			var m = new SquareMatrix(size);
			for (var r = 0; r < size; r++) {
				int d = 1;
				for (var c = 0; c < size; c++) {
					m[r, c] = d;
					d *= r;
				}
			}
			return m;
		}
	}
}
