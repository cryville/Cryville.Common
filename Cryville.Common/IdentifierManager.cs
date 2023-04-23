using System.Collections.Generic;

namespace Cryville.Common {
	/// <summary>
	/// A manager that assigns each given identifiers a unique integer ID.
	/// </summary>
	public class IdentifierManager {
		/// <summary>
		/// A shared instance of the <see cref="IdentifierManager" /> class.
		/// </summary>
		public static IdentifierManager SharedInstance = new IdentifierManager();

		readonly Dictionary<object, int> _idents = new Dictionary<object, int>();
		readonly List<object> _ids = new List<object>();

		readonly object _syncRoot = new object();

		/// <summary>
		/// Creates an instance of the <see cref="IdentifierManager" /> class.
		/// </summary>
		public IdentifierManager() {
			Request(this);
		}

		/// <summary>
		/// Requests an integer ID for an identifier.
		/// </summary>
		/// <param name="ident">The identifier.</param>
		/// <returns>The integer ID.</returns>
		public int Request(object ident) {
			lock (_syncRoot) {
				int id;
				if (!_idents.TryGetValue(ident, out id)) {
					_idents.Add(ident, id = _idents.Count);
					_ids.Add(ident);
				}
				return id;
			}
		}

		/// <summary>
		/// Retrieves the identifier assigned with an integer ID.
		/// </summary>
		/// <param name="id">The integer ID.</param>
		/// <returns>The identifier.</returns>
		public object Retrieve(int id) {
			lock (_syncRoot) {
				return _ids[id];
			}
		}
	}
}
