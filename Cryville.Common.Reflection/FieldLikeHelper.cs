using System;
using System.Reflection;

namespace Cryville.Common.Reflection {
	/// <summary>
	/// Provides a set of <see langword="static" /> methods for field and property.
	/// </summary>
	public static class FieldLikeHelper {
		static readonly object[] emptyObjectArray = { };

		/// <summary>
		/// Finds the member with the specified attribute type in the specified type.
		/// </summary>
		/// <typeparam name="T">The attribute type.</typeparam>
		/// <param name="type">The type containing the member with the specified attribute type.</param>
		/// <returns>The member with the specified attribute type in the specified type. <see langword="null" /> when the member is not found or multiple members are found.</returns>
		public static MemberInfo FindMemberWithAttribute<T>(Type type) where T : Attribute {
			var mil = type.FindMembers(
				MemberTypes.Field | MemberTypes.Property,
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static,
				(m, o) => m.GetCustomAttributes(typeof(T), true).Length != 0,
				null
			);
			if (mil.Length != 1) return null;
			return mil[0];
		}

		/// <summary>
		/// Gets the member from a type with the specified name.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="name">The name of the member.</param>
		/// <returns>The member. <see langword="null" /> when the member is not found or multiple members are found.</returns>
		public static MemberInfo GetMember(Type type, string name) {
			var mil = type.GetMember(
				name,
				MemberTypes.Field | MemberTypes.Property,
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static
			);
			if (mil.Length != 1) return null;
			return mil[0];
		}

		/// <summary>
		/// Gets the type of a member.
		/// </summary>
		/// <param name="mi">The member.</param>
		/// <returns>The type of the member.</returns>
		/// <exception cref="ArgumentException"><paramref name="mi" /> is not a field or a property.</exception>
		public static Type GetMemberType(MemberInfo mi) {
			if (mi is FieldInfo fi) return fi.FieldType;
			if (mi is PropertyInfo pi) return pi.PropertyType;
			else throw new ArgumentException("Member is not field or property.");
		}

		/// <summary>
		/// Gets the value of a member of an object.
		/// </summary>
		/// <param name="mi">The member.</param>
		/// <param name="obj">The object.</param>
		/// <returns>The value.</returns>
		/// <exception cref="ArgumentException"><paramref name="mi" /> is not a field or a property.</exception>
		public static object GetValue(MemberInfo mi, object obj) {
			if (mi is FieldInfo fi) return fi.GetValue(obj);
			else if (mi is PropertyInfo pi) return pi.GetValue(obj, emptyObjectArray);
			else throw new ArgumentException("Member is not field or property.");
		}

		/// <summary>
		/// Sets the value of a member of an object.
		/// </summary>
		/// <param name="mi">The member.</param>
		/// <param name="obj">The object.</param>
		/// <param name="value">The value.</param>
		/// <param name="binder">An optional binder to convert the value.</param>
		/// <exception cref="ArgumentException"><paramref name="mi" /> is not a field or a property.</exception>
		public static void SetValue(MemberInfo mi, object obj, object value, Binder binder = null) {
			if (mi is FieldInfo fi) fi.SetValue(obj, value, BindingFlags.Default, binder, null);
			else if (mi is PropertyInfo pi) pi.SetValue(obj, value, BindingFlags.Default, binder, emptyObjectArray, null);
			else throw new ArgumentException("Member is not field or property.");
		}
	}
}
