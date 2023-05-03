using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Cryville.Common.Interop {
	/// <summary>
	/// Marshals a UTF-8 string to a .NET Framework string, and vice versa.
	/// </summary>
	/// <remarks>
	/// <para>This marshaler is used as a fallback as <c>UnmanagedType.LPUTF8Str</c> does not exist before .NET Framework 4.7.</para>
	/// </remarks>
	public class LPUTF8StrMarshaler : ICustomMarshaler {
		/// <summary>
		/// Returns an instance of the custom marshaler.
		/// </summary>
		/// <param name="cookie">String "cookie" parameter that can be used by the custom marshaler.</param>
		/// <returns>An instance of the custom marshaler.</returns>
		public static ICustomMarshaler GetInstance(string cookie) => new LPUTF8StrMarshaler();

		/// <inheritdoc />
		public void CleanUpManagedData(object ManagedObj) {
			// Do nothing
		}

		/// <inheritdoc />
		public void CleanUpNativeData(IntPtr pNativeData) {
			if (pNativeData == IntPtr.Zero) return;
			Marshal.FreeHGlobal(pNativeData);
		}

		/// <inheritdoc />
		public unsafe int GetNativeDataSize() {
			return sizeof(byte*);
		}

		/// <inheritdoc />
		public unsafe IntPtr MarshalManagedToNative(object ManagedObj) {
			if (ManagedObj == null) return IntPtr.Zero;
			var obj = (string)ManagedObj;
			var buffer = Encoding.UTF8.GetBytes(obj);
			var result = Marshal.AllocHGlobal(buffer.Length + 1);
			Marshal.Copy(buffer, 0, result, buffer.Length);
			var ptr = (byte*)result.ToPointer();
			ptr[buffer.Length] = 0;
			return result;
		}

		/// <inheritdoc />
		public unsafe object MarshalNativeToManaged(IntPtr pNativeData) {
			if (pNativeData == IntPtr.Zero) return null;
			var ptr = (byte*)pNativeData.ToPointer();
			var buffer = new List<byte>();
			while (*ptr != 0) {
				buffer.Add(*ptr);
				ptr++;
			}
			return Encoding.UTF8.GetString(buffer.ToArray());
		}
	}
}
