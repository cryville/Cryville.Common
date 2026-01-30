#if NET5_0_OR_GREATER
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Runtime.CompilerServices.IsExternalInit))]
#else
using System.ComponentModel;

namespace System.Runtime.CompilerServices {
	/// <summary>
	/// Marks a property setter as external-init.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class IsExternalInit { }
}
#endif
