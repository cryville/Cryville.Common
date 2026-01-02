#if !NETSTANDARD2_1_OR_GREATER && !NETCOREAPP3_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis {
	/// <summary>
	/// Specifies that the method will not return if the associated <see cref="bool" /> parameter is passed the specified value.
	/// </summary>
	/// <param name="parameterValue">The condition parameter value. Code after the method is considered unreachable by diagnostics if the argument to the associated parameter matches this value.</param>
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	public sealed class DoesNotReturnIfAttribute(bool parameterValue) : Attribute {
		/// <summary>
		/// Gets the condition parameter value.
		/// </summary>
		public bool ParameterValue { get; } = parameterValue;
	}
}
#endif
