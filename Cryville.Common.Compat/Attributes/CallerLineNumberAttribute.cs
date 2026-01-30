#if NET45_OR_GREATER || NETSTANDARD1_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Runtime.CompilerServices.CallerLineNumberAttribute))]
#else
namespace System.Runtime.CompilerServices {
	/// <summary>
	/// Tags parameter that should be filled with specific caller line number.
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	public sealed class CallerLineNumberAttribute : Attribute { }
}
#endif
