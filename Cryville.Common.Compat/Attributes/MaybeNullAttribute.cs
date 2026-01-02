#if !NETSTANDARD2_1_OR_GREATER && !NETCOREAPP3_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis {
	/// <summary>
	/// Specifies that an output may be <see langword="null" /> even if the corresponding type disallows it.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
	public sealed class MaybeNullAttribute : Attribute { }
}
#endif
