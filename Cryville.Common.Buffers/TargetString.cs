using System;
using System.Collections;
using System.Collections.Generic;

namespace Cryville.Common.Buffers {
	/// <summary>
	/// An auto-resized <see cref="char" /> array as a variable-length string used as a target that is modified frequently.
	/// </summary>
	public class TargetString : IEnumerable<char> {
		/// <summary>
		/// Occurs when <see cref="Validate" /> is called if the string is invalidated.
		/// </summary>
		public event Action? Updated;
		char[] _arr;
		bool _invalidated;
		/// <summary>
		/// Creates an instance of the <see cref="TargetString" /> class with a capacity of 16.
		/// </summary>
		public TargetString() : this(16) { }
		/// <summary>
		/// Creates an instance of the <see cref="TargetString" /> class.
		/// </summary>
		/// <param name="capacity">The initial capacity of the string.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity" /> is less than or equal to 0.</exception>
		public TargetString(int capacity) {
			if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
			_arr = new char[capacity];
		}
		/// <summary>
		/// Gets or sets one of the characters in the string.
		/// </summary>
		/// <param name="index">The zero-based index of the character.</param>
		/// <returns>The character at the given index.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is less than 0 or not less than <see cref="Length" />.</exception>
		/// <remarks>
		/// <para>Set <see cref="Length" /> to a desired value before updating the characters.</para>
		/// <para>Call <see cref="Validate" /> after all the characters are updated.</para>
		/// <para>Changing any character invalidates the string.</para>
		/// </remarks>
		public char this[int index] {
			get {
				if (index < 0 || index >= m_length)
					throw new ArgumentOutOfRangeException(nameof(index));
				return _arr[index];
			}
			set {
				if (index < 0 || index >= m_length)
					throw new ArgumentOutOfRangeException(nameof(index));
				if (_arr[index] == value) return;
				_arr[index] = value;
				_invalidated = true;
			}
		}
		int m_length;
		/// <summary>
		/// The length of the string.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">The value specified for a set operation is less than 0.</exception>
		/// <remarks>
		/// <para>Changing the length of the string invalidates the string.</para>
		/// </remarks>
		public int Length {
			get {
				return m_length;
			}
			set {
				if (Length < 0) throw new ArgumentOutOfRangeException(nameof(value));
				if (m_length == value) return;
				if (_arr.Length < value) {
					var len = _arr.Length;
					while (len < value) len *= 2;
					var arr2 = new char[len];
					Array.Copy(_arr, arr2, m_length);
					_arr = arr2;
				}
				m_length = value;
				_invalidated = true;
			}
		}
		/// <summary>
		/// Validates the string.
		/// </summary>
		public void Validate() {
			if (!_invalidated) return;
			_invalidated = false;
			Updated?.Invoke();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the <see cref="TargetString" />.
		/// </summary>
		/// <returns>A <see cref="Enumerator" /> for the <see cref="TargetString" />.</returns>
		public Enumerator GetEnumerator() {
			return new Enumerator(this);
		}
		IEnumerator<char> IEnumerable<char>.GetEnumerator() {
			return new Enumerator(this);
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return new Enumerator(this);
		}

		/// <inheritdoc />
		public struct Enumerator : IEnumerator<char> {
			readonly TargetString _self;
			int _index;
			internal Enumerator(TargetString self) {
				_self = self;
				_index = -1;
			}

			/// <inheritdoc />
			public readonly char Current {
				get {
					if (_index < 0)
						throw new InvalidOperationException(_index == -1 ? "Enum not started" : "Enum ended");
					return _self[_index];
				}
			}

			readonly object IEnumerator.Current => Current;

			/// <inheritdoc />
			public void Dispose() => _index = -2;

			/// <inheritdoc />
			public bool MoveNext() {
				if (_index == -2) return false;
				_index++;
				if (_index >= _self.Length) {
					_index = -2;
					return false;
				}
				return true;
			}

			/// <inheritdoc />
			public void Reset() => _index = -1;
		}
	}
}
