#if NETCOREAPP3_0_OR_GREATER
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Runtime.CompilerServices.CallerArgumentExpressionAttribute))]
#else
namespace System.Runtime.CompilerServices {
	/// <summary>
	/// Indicates that a parameter captures the expression passed for another parameter as a string.
	/// </summary>
	/// <remarks>
	/// This attribute is implemented in the compiler for C# 10 and later versions only.
	/// </remarks>
	/// <param name="parameterName">The name of the parameter whose expression should be captured as a string.</param>
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public sealed class CallerArgumentExpressionAttribute(string parameterName) : Attribute {
		/// <summary>
		/// Gets the name of the parameter whose expression should be captured as a string.
		/// </summary>
		public string ParameterName { get; } = parameterName;
	}
}
#endif
