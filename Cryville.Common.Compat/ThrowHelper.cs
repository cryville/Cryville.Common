using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

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
#if NET6_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
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
#if NET7_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void ThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
#if NET7_0_OR_GREATER
			ArgumentException.ThrowIfNullOrEmpty(argument, paramName);
#else
			if (IsNullOrEmpty(argument)) {
				ThrowIfNull(argument, paramName);
				throw new ArgumentException("The value cannot be an empty string.", paramName);
			}
			[MethodImpl(EnumValues.MethodImplOptionsAggressiveInlining)]
			static bool IsNullOrEmpty([NotNullWhen(false)] string? value) {
				return string.IsNullOrEmpty(value);
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
#if NET8_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
#if NET8_0_OR_GREATER
			ArgumentException.ThrowIfNullOrWhiteSpace(argument, paramName);
#else
			if (IsNullOrWhiteSpace(argument)) {
				ThrowIfNull(argument, paramName);
				throw new ArgumentException("The value cannot be an empty string or composed entirely of whitespace.", paramName);
			}
			[MethodImpl(EnumValues.MethodImplOptionsAggressiveInlining)]
			static bool IsNullOrWhiteSpace([NotNullWhen(false)] string? value) {
#if NET40_OR_GREATER || NETSTANDARD1_0_OR_GREATER || NETCORE1_0_OR_GREATER
				return string.IsNullOrWhiteSpace(value);
#else
				if (value == null) return true;
				foreach (var c in value) {
					if (!char.IsWhiteSpace(c)) return false;
				}
				return true;
#endif
			}
#endif
		}

		/// <summary>
		/// Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is equal to <paramref name="other" />.
		/// </summary>
		/// <typeparam name="T">The type of the objects to validate.</typeparam>
		/// <param name="value">The argument to validate as not equal to <paramref name="other" />.</param>
		/// <param name="other">The value to compare with <paramref name="value" />.</param>
		/// <param name="paramName">The name of the parameter with which <paramref name="value" /> corresponds.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is equal to <paramref name="other" />.</exception>
#if NET8_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void ThrowIfEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : IEquatable<T>? {
#if NET8_0_OR_GREATER
			ArgumentOutOfRangeException.ThrowIfEqual(value, other, paramName);
#else
			if (EqualityComparer<T>.Default.Equals(value, other)) {
				throw new ArgumentOutOfRangeException(
					paramName, value,
					string.Format(CultureInfo.InvariantCulture, "{0} ('{1}') must not be equal to '{2}'.", paramName, (object?)value ?? "null", (object?)other ?? "null")
				);
			}
#endif
		}

		/// <summary>
		/// Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is not equal to <paramref name="other" />.
		/// </summary>
		/// <typeparam name="T">The type of the objects to validate.</typeparam>
		/// <param name="value">The argument to validate as equal to <paramref name="other" />.</param>
		/// <param name="other">The value to compare with <paramref name="value" />.</param>
		/// <param name="paramName">The name of the parameter with which <paramref name="value" /> corresponds.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is not equal to <paramref name="other" />.</exception>
#if NET8_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void ThrowIfNotEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : IEquatable<T>? {
#if NET8_0_OR_GREATER
			ArgumentOutOfRangeException.ThrowIfNotEqual(value, other, paramName);
#else
			if (!EqualityComparer<T>.Default.Equals(value, other)) {
				throw new ArgumentOutOfRangeException(
					paramName, value,
					string.Format(CultureInfo.InvariantCulture, "{0} ('{1}') must be equal to '{2}'.", paramName, (object?)value ?? "null", (object?)other ?? "null")
				);
			}
#endif
		}

		/// <summary>
		/// Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is greater than <paramref name="other" />.
		/// </summary>
		/// <typeparam name="T">The type of the objects to validate.</typeparam>
		/// <param name="value">The argument to validate as less than or equal to <paramref name="other" />.</param>
		/// <param name="other">The value to compare with <paramref name="value" />.</param>
		/// <param name="paramName">The name of the parameter with which <paramref name="value" /> corresponds.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is greater than <paramref name="other" />.</exception>
#if NET8_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void ThrowIfGreaterThan<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : IComparable<T> {
#if NET8_0_OR_GREATER
			ArgumentOutOfRangeException.ThrowIfGreaterThan(value, other, paramName);
#else
			if (value.CompareTo(other) > 0) {
				throw new ArgumentOutOfRangeException(
					paramName, value,
					string.Format(CultureInfo.InvariantCulture, "{0} ('{1}') must be less than or equal to '{2}'.", paramName, (object?)value ?? "null", (object?)other ?? "null")
				);
			}
#endif
		}

		/// <summary>
		/// Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is greater than or equal to <paramref name="other" />.
		/// </summary>
		/// <typeparam name="T">The type of the objects to validate.</typeparam>
		/// <param name="value">The argument to validate as less than <paramref name="other" />.</param>
		/// <param name="other">The value to compare with <paramref name="value" />.</param>
		/// <param name="paramName">The name of the parameter with which <paramref name="value" /> corresponds.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is greater than or equal to <paramref name="other" />.</exception>
#if NET8_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void ThrowIfGreaterThanOrEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : IComparable<T> {
#if NET8_0_OR_GREATER
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, other, paramName);
#else
			if (value.CompareTo(other) >= 0) {
				throw new ArgumentOutOfRangeException(
					paramName, value,
					string.Format(CultureInfo.InvariantCulture, "{0} ('{1}') must be less than '{2}'.", paramName, (object?)value ?? "null", (object?)other ?? "null")
				);
			}
#endif
		}

		/// <summary>
		/// Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is less than <paramref name="other" />.
		/// </summary>
		/// <typeparam name="T">The type of the objects to validate.</typeparam>
		/// <param name="value">The argument to validate as greater than or equal to <paramref name="other" />.</param>
		/// <param name="other">The value to compare with <paramref name="value" />.</param>
		/// <param name="paramName">The name of the parameter with which <paramref name="value" /> corresponds.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is less than <paramref name="other" />.</exception>
#if NET8_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void ThrowIfLessThan<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : IComparable<T> {
#if NET8_0_OR_GREATER
			ArgumentOutOfRangeException.ThrowIfLessThan(value, other, paramName);
#else
			if (value.CompareTo(other) < 0) {
				throw new ArgumentOutOfRangeException(
					paramName, value,
					string.Format(CultureInfo.InvariantCulture, "{0} ('{1}') must be greater than or equal to '{2}'.", paramName, (object?)value ?? "null", (object?)other ?? "null")
				);
			}
#endif
		}

		/// <summary>
		/// Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="value" /> is less than or equal to <paramref name="other" />.
		/// </summary>
		/// <typeparam name="T">The type of the objects to validate.</typeparam>
		/// <param name="value">The argument to validate as greater than <paramref name="other" />.</param>
		/// <param name="other">The value to compare with <paramref name="value" />.</param>
		/// <param name="paramName">The name of the parameter with which <paramref name="value" /> corresponds.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is less than or equal to <paramref name="other" />.</exception>
#if NET8_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void ThrowIfLessThanOrEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : IComparable<T> {
#if NET8_0_OR_GREATER
			ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, other, paramName);
#else
			if (value.CompareTo(other) <= 0) {
				throw new ArgumentOutOfRangeException(
					paramName, value,
					string.Format(CultureInfo.InvariantCulture, "{0} ('{1}') must be greater than '{2}'.", paramName, (object?)value ?? "null", (object?)other ?? "null")
				);
			}
#endif
		}
	}
}
