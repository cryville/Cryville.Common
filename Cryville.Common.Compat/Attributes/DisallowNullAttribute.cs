#if !NETSTANDARD2_1_OR_GREATER && !NETCOREAPP3_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis {
	/// <summary>
	/// Specifies that <see langword="null" /> is disallowed as an input even if the corresponding type allows it.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	public sealed class DisallowNullAttribute : Attribute { }
}
#endif
