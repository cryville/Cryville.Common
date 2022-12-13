using System;
using System.Collections;
using System.Collections.Generic;

namespace Cryville.Common.Buffers {
	/// <summary>
	/// An auto-resized <see cref="char" /> array as a variable-length string used as a target that is modified frequently.
	/// </summary>
	public class TargetString : IEnumerable<char> {
		public event Action OnUpdate;
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
		public TargetString(int capacity) {
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
		/// <para>Call <see cref=" Validate" /> after all the characters are updated.</para>
		/// </remarks>
		public char this[int index] {
			get {
				if (index < 0 || index >= m_length)
					throw new ArgumentOutOfRangeException("index");
				return _arr[index];
			}
			set {
				if (index < 0 || index >= m_length)
					throw new ArgumentOutOfRangeException("index");
				if (_arr[index] == value) return;
				_arr[index] = value;
				_invalidated = true;
			}
		}
		int m_length;
		/// <summary>
		/// The length of the string.
		/// </summary>
		public int Length {
			get {
				return m_length;
			}
			set {
				if (m_length == value) return;
				if (_arr.Length < value) {
					var len = m_length;
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
			var ev = OnUpdate;
			if (ev != null) ev.Invoke();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
		public IEnumerator<char> GetEnumerator() {
			return new Enumerator(this);
		}

		class Enumerator : IEnumerator<char> {
			readonly TargetString _self;
			int _index = -1;
			public Enumerator(TargetString self) { _self = self; }

			public char Current {
				get {
					if (_index < 0)
						throw new InvalidOperationException(_index == -1 ? "Enum not started" : "Enum ended");
					return _self[_index];
				}
			}

			object IEnumerator.Current { get { return Current; } }

			public void Dispose() {
				_index = -2;
			}

			public bool MoveNext() {
				if (_index == -2) return false;
				_index++;
				if (_index >= _self.Length) {
					_index = -2;
					return false;
				}
				return true;
			}

			public void Reset() {
				_index = -1;
			}
		}
	}
}
