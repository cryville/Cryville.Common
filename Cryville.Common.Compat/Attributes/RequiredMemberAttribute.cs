#if NET7_0_OR_GREATER
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Runtime.CompilerServices.RequiredMemberAttribute))]
#else
using System.ComponentModel;

namespace System.Runtime.CompilerServices {
	/// <summary>
	/// Specifies that a type has required members or that a member is required.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class RequiredMemberAttribute : Attribute { }
}
#endif
