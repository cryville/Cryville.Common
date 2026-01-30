#if NET7_0_OR_GREATER
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Diagnostics.CodeAnalysis.SetsRequiredMembersAttribute))]
#else
namespace System.Diagnostics.CodeAnalysis {
	/// <summary>
	/// Specifies that this constructor sets all required members for the current type, and callers do not need to set any required members themselves.
	/// </summary>
	[AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
	public sealed class SetsRequiredMembersAttribute : Attribute { }
}
#endif
