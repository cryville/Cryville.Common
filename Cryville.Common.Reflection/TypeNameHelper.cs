using System;
using System.Linq;

namespace Cryville.Common.Reflection {
	/// <summary>
	/// Provides a set of <see langword="static" /> methods for getting type name.
	/// </summary>
	public static class TypeNameHelper {
		/// <summary>
		/// Gets a simple name of a type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>A simple name of the class.</returns>
		public static string GetSimpleName(Type type) {
			string result = type.Name;
			var typeArgs = type.GetGenericArguments();
			if (typeArgs.Length > 0) {
				result = string.Format("{0}[{1}]", result, string.Join(",", (from a in typeArgs select GetSimpleName(a)).ToArray()));
			}
			return result;
		}

		/// <summary>
		/// Gets the namespace qualified name of a type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The namespace qualified name of the class.</returns>
		public static string GetNamespaceQualifiedName(Type type) {
			string result = type.Namespace + "." + type.Name;
			var typeArgs = type.GetGenericArguments();
			if (typeArgs.Length > 0) {
				result = string.Format("{0}[{1}]", result, string.Join(",", (from a in typeArgs select GetNamespaceQualifiedName(a)).ToArray()));
			}
			return result;
		}
	}
}
