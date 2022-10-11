using System;
using System.Text;

namespace Cryville.Common {
	/// <summary>
	/// Provides a set of <see langword="static" /> methods related to string operations.
	/// </summary>
	public static class StringUtils {
		/// <summary>
		/// Removes the extension in a file name or file path.
		/// </summary>
		/// <param name="s">The file name or file path.</param>
		/// <returns>The file name or file path with the extension removed.</returns>
		public static string TrimExt(string s) {
			return s.Substring(0, s.LastIndexOf("."));
		}
		/// <summary>
		/// Converts the value of a <see cref="TimeSpan" /> to a human-readable string.
		/// </summary>
		/// <param name="timeSpan">The time span.</param>
		/// <param name="digits">The digit count for seconds.</param>
		/// <returns>A human-readable string representing the time span.</returns>
		public static string ToString(this TimeSpan timeSpan, int digits) {
			var b = new StringBuilder();
			bool flag = false;
			if (timeSpan.TotalDays >= 1) {
				flag = true;
				b.Append(timeSpan.Days.ToString() + ":");
			}
			if (flag || timeSpan.TotalHours >= 1)
				b.Append((timeSpan.Hours % 24).ToString() + ":");
			b.Append((timeSpan.Minutes % 60).ToString("00") + ":");
			b.Append((timeSpan.TotalSeconds % 60).ToString("00." + new string('0', digits)));
			return b.ToString();
		}
	}
}
