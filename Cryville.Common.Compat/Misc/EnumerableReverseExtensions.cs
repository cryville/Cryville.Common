#if NET35_OR_GREATER || NETSTANDARD || (NETCOREAPP && !NET10_0_OR_GREATER)
using Cryville.Common.Compat;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Linq {
	/// <summary>
	/// Provides a set of extension methods that fixes a <see href="https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/breaking-changes/compiler%20breaking%20changes%20-%20dotnet%2010#enumerablereverse">breaking change</see> introduced in C# 14 involving <see cref="Enumerable.Reverse{TSource}(IEnumerable{TSource})" />.
	/// </summary>
	public static class EnumerableReverseExtensions {
		/// <summary>
		/// Inverts the order of the elements in an array.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <param name="source">An array of values to reverse.</param>
		/// <returns>An array whose elements correspond to those of the input array in reverse order.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source" /> is null.</exception>
		[MethodImpl(EnumValues.MethodImplOptionsAggressiveInlining)]
		public static IEnumerable<TSource> Reverse<TSource>(this TSource[] source) {
			return Enumerable.Reverse(source);
		}
	}
}
#endif
