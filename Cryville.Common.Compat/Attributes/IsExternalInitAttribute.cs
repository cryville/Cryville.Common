#if !NET5_0_OR_GREATER
using System.ComponentModel;

namespace System.Runtime.CompilerServices {
	/// <summary>
	/// Marks a property setter as external-init.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class IsExternalInit { }
}
#endif
