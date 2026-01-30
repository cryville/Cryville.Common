#if NET45_OR_GREATER || NETSTANDARD1_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Runtime.CompilerServices.CallerFilePathAttribute))]
#else
namespace System.Runtime.CompilerServices {
	/// <summary>
	/// Tags parameter that should be filled with specific caller source file path.
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	public sealed class CallerFilePathAttribute : Attribute { }
}
#endif
