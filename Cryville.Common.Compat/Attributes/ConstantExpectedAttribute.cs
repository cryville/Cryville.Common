#if !NET7_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis {
	/// <summary>
	/// Indicates that the specified method parameter expects a constant.
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	public sealed class ConstantExpectedAttribute : Attribute {
		/// <summary>
		/// Gets or sets the minimum bound of the expected constant, inclusive.
		/// </summary>
		public object? Min { get; set; }

		/// <summary>
		/// Gets or sets the maximum bound of the expected constant, inclusive.
		/// </summary>
		public object? Max { get; set; }
	}
}
#endif
