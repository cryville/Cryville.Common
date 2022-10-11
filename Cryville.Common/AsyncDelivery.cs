using System;

namespace Cryville.Common {
	/// <summary>
	/// Represents a cancellable asynchronized task.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class AsyncDelivery<T> {
		/// <summary>
		/// The delegate to cancel the task.
		/// </summary>
		public Action CancelSource { private get; set; }
		/// <summary>
		/// The delegate to call on task completion.
		/// </summary>
		public Action<bool, T> Destination { private get; set; }
		/// <summary>
		/// Delivers the result to the destination.
		/// </summary>
		/// <param name="succeeded">Whether the task has succeeded.</param>
		/// <param name="result">The result.</param>
		public void Deliver(bool succeeded, T result) {
			if (Destination != null) Destination(succeeded, result);
		}
		/// <summary>
		/// Cancels the task.
		/// </summary>
		public void Cancel() {
			if (CancelSource != null) CancelSource();
		}
	}
}
