using System;
using UnityEngine;

namespace Cryville.Common.Unity {
	/// <summary>
	/// Provides a set of <see langword="static" /> methods for serializing and deserializing Unity objects.
	/// </summary>
	public static class SerializationExtensions {
		/// <summary>
		/// Converts a <see cref="Single" /> array to <see cref="Color" />.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <returns>The converted <see cref="Color" />.</returns>
		/// <exception cref="ArgumentException">The length of <paramref name="array" /> is not 3 or 4.</exception>
		/// <remarks>This method converts the color in HSV color space. The ranges for the color components are: H [0..360]; S [0..100]; V [0..100].</remarks>
		public static Color ToColor(this float[] array) {
			if (array.Length == 3) return Color.HSVToRGB(array[0] / 360, array[1] / 100, array[2] / 100);
			else if (array.Length == 4) {
				var col = Color.HSVToRGB(array[0] / 360, array[1] / 100, array[2] / 100);
				col.a = array[3] / 100;
				return col;
			}
			else throw new ArgumentException("Array length not 3 or 4");
		}

		/// <summary>
		/// Converts a <see cref="Single" /> array to <see cref="Vector2" />.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <returns>The converted <see cref="Vector2" />.</returns>
		/// <exception cref="ArgumentException">The length of <paramref name="array" /> is not 2.</exception>
		public static Vector2 ToVector2(this float[] array) {
			if (array.Length == 2) return new Vector2(array[0], array[1]);
			else throw new ArgumentException("Array length not 2");
		}
	}
}
