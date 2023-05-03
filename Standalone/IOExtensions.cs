using System.IO;
using System.Text;

namespace Cryville.Common {
	/// <summary>
	/// Provides a set of <see langword="static" /> methods related to file system and IO.
	/// </summary>
	public static class IOExtensions {
		/// <summary>
		/// Gets a subdirectory of a directory. The subdirectory is created if it does not exist.
		/// </summary>
		/// <param name="dir">The parent directory.</param>
		/// <param name="name">The name of the subdirectory.</param>
		/// <returns></returns>
		public static DirectoryInfo GetSubdirectory(this DirectoryInfo dir, string name) {
			var l1 = dir.GetDirectories(name);
			if (l1.Length == 0) return dir.CreateSubdirectory(name);
			else return l1[0];
		}

		/// <summary>
		/// Reads a string length-prefixed with a <see cref="System.UInt16" />.
		/// </summary>
		/// <param name="reader">The binary reader.</param>
		/// <param name="encoding">The encoding of the string.</param>
		/// <returns>The string read from the reader.</returns>
		public static string ReadUInt16String(this BinaryReader reader, Encoding encoding = null) {
			if (encoding == null) encoding = Encoding.UTF8;
			var len = reader.ReadUInt16();
			byte[] buffer = reader.ReadBytes(len);
			return encoding.GetString(buffer);
		}

		/// <summary>
		/// Writes a string length-prefixed with a <see cref="System.UInt16" />.
		/// </summary>
		/// <param name="writer">The binary writer.</param>
		/// <param name="value">The string to write by the writer.</param>
		/// <param name="encoding">The encoding of the string.</param>
		public static void WriteUInt16String(this BinaryWriter writer, string value, Encoding encoding = null) {
			if (encoding == null) encoding = Encoding.UTF8;
			byte[] buffer = encoding.GetBytes(value);
			writer.Write((ushort)buffer.Length);
			writer.Write(buffer);
		}
	}
}
