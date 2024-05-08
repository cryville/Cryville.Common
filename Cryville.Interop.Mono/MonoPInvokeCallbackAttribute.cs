using System;

namespace Cryville.Interop.Mono {
	/// <summary>
	/// Attribute used to annotate functions that will be called back from the unmanaged world.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class MonoPInvokeCallbackAttribute : Attribute {
		/// <summary>
		/// The type of the delegate that will be calling us back.
		/// </summary>
		public Type? DelegateType { get; }

		/// <summary>
		/// Constructor for the MonoPInvokeCallbackAttribute.
		/// </summary>
		[Obsolete("Use the parameterized constructor for Xamarin support.")]
		public MonoPInvokeCallbackAttribute() { }
		/// <summary>
		/// Constructor for the MonoPInvokeCallbackAttribute.
		/// </summary>
		/// <param name="delegateType">The type of the delegate that will be calling us back.</param>
		public MonoPInvokeCallbackAttribute(Type delegateType) { DelegateType = delegateType; }
	}
}
