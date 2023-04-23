using UnityEngine;
using UnityEngine.UI;

namespace Cryville.Common.Unity.UI {
	/// <summary>
	/// Fits the length of an axis of an element with respect to the children count and the shared aspect ratio.
	/// </summary>
	public class LayoutAspectRatioFitter : MonoBehaviour {
		[SerializeField]
		[Tooltip("The aspect ratio per element.")]
		private float m_aspectRatioPerElement = 1;
		/// <summary>
		/// The aspect ratio per element.
		/// </summary>
		public float AspectRatioPerElement {
			get { return m_aspectRatioPerElement; }
			set { m_aspectRatioPerElement = value; }
		}

		AspectRatioFitter aspectRatioFitter;
		DockAspectRatioLayoutGroup syncTo;
		int axis;
#pragma warning disable IDE0051
		void Awake() {
			aspectRatioFitter = GetComponent<AspectRatioFitter>();
			if (aspectRatioFitter == null) {
				syncTo = GetComponentInParent<DockAspectRatioLayoutGroup>();
				axis = (syncTo.DockSide == DockLayoutGroup.Side.Top || syncTo.DockSide == DockLayoutGroup.Side.Bottom) ? 1 : 0;
			}
			else axis = aspectRatioFitter.aspectMode == AspectRatioFitter.AspectMode.WidthControlsHeight ? 1 : 0;
			OnTransformChildrenChanged();
		}
		void OnTransformChildrenChanged() {
			float r;
			switch (axis) {
				case 0:
					r = AspectRatioPerElement * transform.childCount;
					break;
				case 1:
					r = AspectRatioPerElement / transform.childCount;
					break;
				default: return;
			}
			if (aspectRatioFitter != null) aspectRatioFitter.aspectRatio = r;
			else syncTo.DockAspectRatio = r;
		}
#pragma warning restore IDE0051
	}
}
