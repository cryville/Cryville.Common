#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute))]
#else
namespace System.Diagnostics.CodeAnalysis {
	/// <summary>
	/// Specifies that the output will be non-null if the named parameter is non-null.
	/// </summary>
	/// <param name="parameterName">The associated parameter name. The output will be non-null if the argument to the parameter specified is non-null.</param>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	public sealed class NotNullIfNotNullAttribute(string parameterName) : Attribute {
		/// <summary>
		/// Gets the associated parameter name.
		/// </summary>
		public string ParameterName { get; } = parameterName;
	}
}
#endif
