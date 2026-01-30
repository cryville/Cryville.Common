#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute))]
#else
namespace System.Diagnostics.CodeAnalysis {
	/// <summary>
	/// Specifies that when a method returns <see cref="ReturnValue" />, the parameter may be <see langword="null" /> even if the corresponding type disallows it.
	/// </summary>
	/// <param name="returnValue">The return value condition. If the method returns this value, the associated parameter may be <see langword="null" />.</param>
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	public sealed class MaybeNullWhenAttribute(bool returnValue) : Attribute {
		/// <summary>
		/// Gets the return value condition.
		/// </summary>
		public bool ReturnValue { get; } = returnValue;
	}
}
#endif
