using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cryville.Common.Unity.UI {
	/// <summary>
	/// A <see cref="ILayoutElement" /> that takes the length of one axis to compute the preferred length of the other axis with respect to a aspect ratio.
	/// </summary>
	public class AspectRatioLayoutElement : UIBehaviour, ILayoutElement {
		[SerializeField]
		[Tooltip("The aspect ratio. Width divided by height.")]
		private float m_aspectRatio = 1;
		/// <summary>
		/// The aspect ratio. Width divided by height.
		/// </summary>
		public float AspectRatio {
			get { return m_aspectRatio; }
			set { SetProperty(ref m_aspectRatio, value); }
		}

		[SerializeField]
		[Tooltip("Whether to compute the length of the y axis.")]
		private bool m_isVertical = false;
		/// <summary>
		/// Whether to compute the length of the y axis.
		/// </summary>
		public bool IsVertical {
			get { return m_isVertical; }
			set { SetProperty(ref m_isVertical, value); }
		}

		private void SetProperty<T>(ref T prop, T value) {
			if (Equals(prop, value)) return;
			prop = value;
			SetDirty();
		}

		private void SetDirty() {
			if (!IsActive()) return;
			LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
		}

		/// <inheritdoc />
		public float minWidth {
			get {
				return m_isVertical ? 0 : (transform as RectTransform).rect.height * m_aspectRatio;
			}
		}
		/// <inheritdoc />
		public float preferredWidth { get { return minWidth; } }
		/// <inheritdoc />
		public float flexibleWidth { get { return 0; } }

		/// <inheritdoc />
		public float minHeight {
			get {
				return m_isVertical ? (transform as RectTransform).rect.width / m_aspectRatio : 0;
			}
		}
		/// <inheritdoc />
		public float preferredHeight { get { return minHeight; } }
		/// <inheritdoc />
		public float flexibleHeight { get { return 0; } }

		/// <inheritdoc />
		public int layoutPriority { get { return 1; } }

		/// <inheritdoc />
		public void CalculateLayoutInputHorizontal() { }

		/// <inheritdoc />
		public void CalculateLayoutInputVertical() { }

		protected override void OnDidApplyAnimationProperties() {
			base.OnDidApplyAnimationProperties();
			SetDirty();
		}

		protected override void OnDisable() {
			SetDirty();
			base.OnDisable();
		}

		protected override void OnEnable() {
			base.OnEnable();
			SetDirty();
		}

		protected override void OnRectTransformDimensionsChange() {
			base.OnRectTransformDimensionsChange();
			SetDirty();
		}

		protected override void OnTransformParentChanged() {
			base.OnTransformParentChanged();
			SetDirty();
		}

		// Overriding fails compiler
		new void OnValidate() {
			SetDirty();
		}
	}
}
