#if NET7_0_OR_GREATER
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Runtime.CompilerServices.CompilerFeatureRequiredAttribute))]
#else
namespace System.Runtime.CompilerServices {
	/// <summary>
	/// Indicates that compiler support for a particular feature is required for the location where this attribute is applied.
	/// </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
	public sealed class CompilerFeatureRequiredAttribute : Attribute {
		/// <summary>
		/// The <see cref="FeatureName" /> used for the ref structs C# feature.
		/// </summary>
		public const string RefStructs = "RefStructs";

		/// <summary>
		/// The <see cref="FeatureName" /> used for the required members C# feature.
		/// </summary>
		public const string RequiredMembers = "RequiredMembers";

		/// <summary>
		/// The name of the compiler feature.
		/// </summary>
		public string FeatureName { get; }

		/// <summary>
		/// If true, the compiler can choose to allow access to the location where this attribute is applied if it does not understand <see cref="FeatureName" />.
		/// </summary>
		public bool IsOptional { get; init; }

		/// <summary>
		/// Creates an instance of the <see cref="CompilerFeatureRequiredAttribute" /> class.
		/// </summary>
		/// <param name="featureName">The name of the compiler feature.</param>
		public CompilerFeatureRequiredAttribute(string featureName) {
			FeatureName = featureName;
		}
	}
}
#endif
