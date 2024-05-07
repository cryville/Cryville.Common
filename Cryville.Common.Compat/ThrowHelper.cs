using System;
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace Cryville.Common.Compat {
	/// <summary>
	/// Provides <see langword="static" /> methods for throwing common exceptions.
	/// </summary>
	public static class ThrowHelper {
		/// <summary>
		/// Throws an <see cref="ArgumentNullException" /> if <paramref name="argument" /> is <see langword="null" />.
		/// </summary>
		/// <param name="argument">The reference type argument to validate as non-null.</param>
		/// <param name="paramName">The name of the parameter with which <paramref name="argument" /> corresponds. If you omit this parameter, the name of <paramref name="argument" /> is used.</param>
		/// <exception cref="ArgumentNullException"><paramref name="argument" /> is <see langword="null" />.</exception>
		public static void ThrowIfNull(
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
			[NotNull]
#endif
			object? argument,
#if NETCOREAPP3_0_OR_GREATER
			[CallerArgumentExpression("argument")]
#endif
			string? paramName = null
		) {
#if NET6_0_OR_GREATER
			ArgumentNullException.ThrowIfNull(argument, paramName);
#else
			if (argument == null) throw new ArgumentNullException(paramName);
#endif
		}

		/// <summary>
		/// Throws an exception if <paramref name="argument" /> is <see langword="null" /> or empty.
		/// </summary>
		/// <param name="argument">The string argument to validate as non-<see langword="null" /> and non-empty.</param>
		/// <param name="paramName">The name of the parameter with which <paramref name="argument" /> corresponds.</param>
		/// <exception cref="ArgumentNullException"><paramref name="argument" /> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentException"><paramref name="argument" /> is empty.</exception>
		public static void ThrowIfNullOrEmpty(
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
			[NotNull]
#endif
			string? argument,
#if NETCOREAPP3_0_OR_GREATER
			[CallerArgumentExpression("argument")]
#endif
			string? paramName = null
		) {
#if NET7_0_OR_GREATER
			ArgumentException.ThrowIfNullOrEmpty(argument, paramName);
#else
			if (string.IsNullOrEmpty(argument)) {
				ThrowIfNull(argument, paramName);
				throw new ArgumentException("Empty string.", paramName);
			}
#endif
		}

		/// <summary>
		/// Throws an exception if <paramref name="argument" /> is <see langword="null" />, empty, or consists only of white-space characters.
		/// </summary>
		/// <param name="argument">The string argument to validate.</param>
		/// <param name="paramName">The name of the parameter with which <paramref name="argument" /> corresponds.</param>
		/// <exception cref="ArgumentNullException"><paramref name="argument" /> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentException"><paramref name="argument" /> is empty or consists only of white-space characters.</exception>
		public static void ThrowIfNullOrWhiteSpace(
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
			[NotNull]
#endif
			string? argument,
#if NETCOREAPP3_0_OR_GREATER
			[CallerArgumentExpression("argument")]
#endif
			string? paramName = null
		) {
#if NET8_0_OR_GREATER
			ArgumentException.ThrowIfNullOrWhiteSpace(argument, paramName);
#else
			if (IsNullOrWhiteSpace(argument)) {
				ThrowIfNull(argument, paramName);
				throw new ArgumentException("Empty or white-space string.", paramName);
			}
#endif
		}

		static bool IsNullOrWhiteSpace(
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
			[NotNullWhen(false)]
#endif
			string? value
		) {
#if NET40_OR_GREATER || NETSTANDARD1_0_OR_GREATER || NETSTANDARD1_0_OR_GREATER
			return string.IsNullOrWhiteSpace(value);
#else
			if (value == null) return true;
			foreach (var c in value) {
				if (!char.IsWhiteSpace(c)) return false;
			}
			return true;
#endif
		}
	}
}
