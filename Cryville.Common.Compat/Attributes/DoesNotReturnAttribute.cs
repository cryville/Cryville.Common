#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute))]
#else
namespace System.Diagnostics.CodeAnalysis {
	/// <summary>
	/// Specifies that a method will never return under any circumstance.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public sealed class DoesNotReturnAttribute : Attribute { }
}
#endif
