using System;

namespace Cryville.Common.Math {
	/// <summary>
	/// Provides a set of <see langword="static" /> methods related to fractions.
	/// </summary>
	public static class FractionUtils {
		/// <summary>
		/// Converts a <see cref="double" /> decimal to a fraction.
		/// </summary>
		/// <param name="value">The decimal.</param>
		/// <param name="error">The error.</param>
		/// <param name="n">The numerator.</param>
		/// <param name="d">The denominator.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is less than 0 or <paramref name="error" /> is not greater than 0 or not less than 1.</exception>
		public static void ToFraction(double value, double error, out int n, out int d) {
			if (value < 0.0)
				throw new ArgumentOutOfRangeException("value", "Must be >= 0.");
			if (error <= 0.0 || error >= 1.0)
				throw new ArgumentOutOfRangeException("accuracy", "Must be > 0 and < 1.");

			int num = (int)System.Math.Floor(value);
			value -= num;

			if (value < error) { n = num; d = 1; return; }
			if (1 - error < value) { n = num + 1; d = 1; return; }

			int lower_n = 0;
			int lower_d = 1;
			int upper_n = 1;
			int upper_d = 1;
			while (true) {
				int middle_n = lower_n + upper_n;
				int middle_d = lower_d + upper_d;

				if (middle_d * (value + error) < middle_n) {
					upper_n = middle_n;
					upper_d = middle_d;
				}
				else if (middle_n < (value - error) * middle_d) {
					lower_n = middle_n;
					lower_d = middle_d;
				}
				else {
					n = num * middle_d + middle_n;
					d = middle_d;
					return;
				}
			}
		}
	}
}
