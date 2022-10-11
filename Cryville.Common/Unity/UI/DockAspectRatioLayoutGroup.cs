using UnityEngine;

namespace Cryville.Common.Unity.UI {
	/// <summary>
	/// A <see cref="DockLayoutGroup" /> that sets the aspect ratio of the docking element.
	/// </summary>
	public sealed class DockAspectRatioLayoutGroup : DockLayoutGroup {
		[SerializeField]
		[Tooltip("The aspect ratio of the docking element.")]
		private float m_dockAspectRatio = 1;
		/// <summary>
		/// The aspect ratio of the docking element.
		/// </summary>
		public float DockAspectRatio {
			get { return m_dockAspectRatio; }
			set { base.SetProperty(ref m_dockAspectRatio, value); }
		}

		protected override float GetDockElementSize(Vector2 groupSize) {
			return groupSize.y * m_dockAspectRatio;
		}
	}
}
