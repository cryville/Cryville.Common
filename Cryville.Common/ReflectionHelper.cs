using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cryville.Common {
	/// <summary>
	/// Provides a set of <see langword="static" /> methods for refletion.
	/// </summary>
	public static class ReflectionHelper {
		static readonly Type[] emptyTypeArray = {};
		/// <summary>
		/// Gets the parameterless constructor of a type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The parameterless constructor of the type.</returns>
		public static ConstructorInfo GetEmptyConstructor(Type type) {
			return type.GetConstructor(emptyTypeArray);
		}
		static readonly object[] emptyObjectArray = {};
		/// <summary>
		/// Invokes the parameterless constructor of a type and returns the result.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The created instance.</returns>
		public static object InvokeEmptyConstructor(Type type) {
			return GetEmptyConstructor(type).Invoke(emptyObjectArray);
		}
		
		/// <summary>
		/// Tries to find a member with the specified attribute type in a type.
		/// </summary>
		/// <typeparam name="T">The attribute type.</typeparam>
		/// <param name="t">The type containing the member with the specified attribute type.</param>
		/// <param name="mi">The member.</param>
		/// <returns>Whether the member is found.</returns>
		public static bool TryFindMemberWithAttribute<T>(Type t, out MemberInfo mi) where T : Attribute {
			try {
				mi = FindMemberWithAttribute<T>(t);
				return true;
			}
			catch (MissingMemberException) {
				mi = null;
				return false;
			}
		}
		/// <summary>
		/// Finds a member with the specified attribute type in a type.
		/// </summary>
		/// <typeparam name="T">The attribute type.</typeparam>
		/// <param name="type">The type containing the member with the specified attribute type.</param>
		/// <returns></returns>
		/// <exception cref="MissingMemberException">The member is not found or multiple members are found.</exception>
		public static MemberInfo FindMemberWithAttribute<T>(Type type) where T : Attribute {
			var mil = type.FindMembers(
				MemberTypes.Field | MemberTypes.Property,
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static,
				(m, o) => m.GetCustomAttributes(typeof(T), true).Length != 0,
				null
			);
			if (mil.Length != 1)
				throw new MissingMemberException(type.Name, typeof(T).Name);
			return mil[0];
		}

		/// <summary>
		/// Gets whether a type is a <see cref="Dictionary{TKey, TValue}" />.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>Whether the type is a <see cref="Dictionary{TKey, TValue}" />.</returns>
		public static bool IsGenericDictionary(Type type) {
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
		}

		/// <summary>
		/// Gets the member from a type with the specified name.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="name">The name of the member.</param>
		/// <returns>The member.</returns>
		/// <exception cref="MissingMemberException">The member is not found or multiple members are found.</exception>
		public static MemberInfo GetMember(Type type, string name) {
			var mil = type.GetMember(
				name,
				MemberTypes.Field | MemberTypes.Property,
				BindingFlags.Public | BindingFlags.Instance
			);
			if (mil.Length != 1)
				throw new MissingMemberException(type.Name, name);
			return mil[0];
		}

		/// <summary>
		/// Gets the type of a member.
		/// </summary>
		/// <param name="mi">The member.</param>
		/// <returns>The type of the member.</returns>
		/// <exception cref="ArgumentException"><paramref name="mi" /> is not a field or a property.</exception>
		public static Type GetMemberType(MemberInfo mi) {
			if (mi is FieldInfo)
				return ((FieldInfo)mi).FieldType;
			if (mi is PropertyInfo)
				return ((PropertyInfo)mi).PropertyType;
			else
				throw new ArgumentException("Member is not field or property.");
		}

		/// <summary>
		/// Gets the value of a member of an object.
		/// </summary>
		/// <param name="mi">The member.</param>
		/// <param name="obj">The object.</param>
		/// <returns>The value.</returns>
		/// <exception cref="ArgumentException"><paramref name="mi" /> is not a field or a property.</exception>
		public static object GetValue(MemberInfo mi, object obj) {
			if (mi is FieldInfo)
				return ((FieldInfo)mi).GetValue(obj);
			else if (mi is PropertyInfo)
				return ((PropertyInfo)mi).GetValue(obj, new object[]{});
			else
				throw new ArgumentException();
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
			if (mi is FieldInfo)
				((FieldInfo)mi).SetValue(obj, value, BindingFlags.Default, binder, null);
			else if (mi is PropertyInfo)
				((PropertyInfo)mi).SetValue(obj, value, BindingFlags.Default, binder, emptyObjectArray, null);
			else
				throw new ArgumentException();
		}

		/// <summary>
		/// Gets all the subclasses of a type in the current app domain.
		/// </summary>
		/// <typeparam name="T">The type.</typeparam>
		/// <returns>An array containing all the subclasses of the type in the current app domain.</returns>
		public static Type[] GetSubclassesOf<T>() where T : class {
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			IEnumerable<Type> r = new List<Type>();
			foreach (var a in assemblies)
				r = r.Concat(a.GetTypes().Where(
					t => t.IsClass
					&& !t.IsAbstract
					&& t.IsSubclassOf(typeof(T))
				));
			return r.ToArray();
		}

		/// <summary>
		/// Gets a simple name of a type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>A simple name of the class.</returns>
		public static string GetSimpleName(Type type) {
			string result = type.Name;
			var typeargs = type.GetGenericArguments();
			if (typeargs.Length > 0) {
				result = string.Format("{0}[{1}]", result, string.Join(",", from a in typeargs select GetSimpleName(a)));
			}
			return result;
		}
	}
}
