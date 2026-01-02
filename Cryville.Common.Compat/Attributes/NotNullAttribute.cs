#if !NETSTANDARD2_1_OR_GREATER && !NETCOREAPP3_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis {
	/// <summary>
	/// Specifies that an output is not <see langword="null" /> even if the corresponding type allows it. Specifies that an input argument was not <see langword="null" /> when the call returns.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
	public sealed class NotNullAttribute : Attribute { }
}
#endif
