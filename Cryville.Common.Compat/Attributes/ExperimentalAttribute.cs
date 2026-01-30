#if NET8_0_OR_GREATER
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Diagnostics.CodeAnalysis.ExperimentalAttribute))]
#else
namespace System.Diagnostics.CodeAnalysis {
	/// <summary>
	/// Indicates that an API is experimental and it may change in the future.
	/// </summary>
	/// <param name="diagnosticId">The ID that the compiler will use when reporting a use of the API the attribute applies to.</param>
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
	public sealed class ExperimentalAttribute(string diagnosticId) : Attribute {
		/// <summary>
		/// Gets the ID that the compiler will use when reporting a use of the API the attribute applies to.
		/// </summary>
		public string DiagnosticId { get; } = diagnosticId;

		/// <summary>
		/// Gets or sets the URL for corresponding documentation. The API accepts a format string instead of an actual URL, creating a generic URL that includes the diagnostic ID.
		/// </summary>
		public string? UrlFormat { get; set; }
	}
}
#endif
