using UnityEngine;

namespace Cryville.Common.Unity.UI {
	/// <summary>
	/// A <see cref="DockLayoutGroup" /> that sets the occupied ratio of the docking element.
	/// </summary>
	public sealed class DockOccupiedRatioLayoutGroup : DockLayoutGroup {
		[SerializeField]
		[Tooltip("The occupied ratio of the docking element.")]
		private float m_dockOccupiedRatio = 1;
		/// <summary>
		/// The occupied ratio of the docking element.
		/// </summary>
		public float DockOccupiedRatio {
			get { return m_dockOccupiedRatio; }
			set { base.SetProperty(ref m_dockOccupiedRatio, value); }
		}

		protected override float GetDockElementSize(Vector2 groupSize) {
			return groupSize.x * m_dockOccupiedRatio;
		}
	}
}
