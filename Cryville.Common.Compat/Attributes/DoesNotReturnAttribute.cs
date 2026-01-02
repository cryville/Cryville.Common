#if !NETSTANDARD2_1_OR_GREATER && !NETCOREAPP3_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis {
	/// <summary>
	/// Specifies that a method will never return under any circumstance.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public sealed class DoesNotReturnAttribute : Attribute { }
}
#endif
