using UnityEngine;
using UnityEngine.UI;

namespace Cryville.Common.Unity.UI {
	/// <summary>
	/// A <see cref="LayoutGroup" /> that docks its first child element to one side.
	/// </summary>
	public abstract class DockLayoutGroup : LayoutGroup {
		/// <summary>
		/// The dock side.
		/// </summary>
		public enum Side {
			/// <summary>
			/// Top.
			/// </summary>
			Top = 0,
			/// <summary>
			/// Right.
			/// </summary>
			Right = 1,
			/// <summary>
			/// Bottom.
			/// </summary>
			Bottom = 2,
			/// <summary>
			/// Left.
			/// </summary>
			Left = 3,
		}
		[SerializeField]
		[Tooltip("The docking side of the first child element.")]
		private Side m_side;
		/// <summary>
		/// The docking side of the first child element.
		/// </summary>
		public Side DockSide {
			get { return m_side; }
			set { base.SetProperty(ref m_side, value); }
		}

		[SerializeField]
		[Tooltip("The slide index. The children slide along the cross axis.")]
		private float m_slideIndex;
		/// <summary>
		/// The slide index. The children slide along the axis.
		/// </summary>
		public float SlideIndex {
			get { return m_slideIndex; }
			set { base.SetProperty(ref m_slideIndex, value); }
		}

		/// <inheritdoc />
		public sealed override void CalculateLayoutInputHorizontal() { base.CalculateLayoutInputHorizontal(); }
		/// <inheritdoc />
		public sealed override void CalculateLayoutInputVertical() { }
		/// <inheritdoc />
		public sealed override void SetLayoutHorizontal() { SetChildrenAlongAxis(0); }
		/// <inheritdoc />
		public sealed override void SetLayoutVertical() { SetChildrenAlongAxis(1); }

		private float GetSlidePosition(float groupHeight, float dockHeight) {
			bool d = Mathf.FloorToInt(m_slideIndex - Mathf.Floor(m_slideIndex / 2) * 2) == 0;
			int l = Mathf.FloorToInt(m_slideIndex / 2);
			float p = m_slideIndex - Mathf.Floor(m_slideIndex);
			if (d) return l * groupHeight + p * dockHeight;
			else return l * groupHeight + dockHeight + p * (groupHeight - dockHeight);
		}

		private void SetChildrenAlongAxis(int axis) {
			int isHorizontal = (int)m_side & 1;
			bool isReversed = m_side == Side.Right || m_side == Side.Bottom;
			var rect = rectTransform.rect;
			if ((isHorizontal ^ axis) == 1) {
				float p0 = isHorizontal == 1 ? m_Padding.left : m_Padding.top;
				float p1 = isHorizontal == 1 ? m_Padding.right : m_Padding.bottom;
				var gs = rect.size - new Vector2(m_Padding.horizontal, m_Padding.vertical);
				if (isHorizontal == 0) gs = new Vector2(gs.y, gs.x);
				float s1 = GetDockElementSize(gs);
				float s0 = GetSlidePosition(gs.x, s1);
				float a1 = (isHorizontal == 0 ? rect.height : rect.width) - p0 - p1;
				for (int i = 0; i < base.rectChildren.Count; i++) {
					var c = base.rectChildren[i];
					bool d = i % 2 == 0;
					int l = i / 2;
					if (isReversed)
						base.SetChildAlongAxis(c, axis, (d ? a1 - s1 + p0 : p0) - a1 * l + s0, d ? s1 : a1 - s1);
					else
						base.SetChildAlongAxis(c, axis, (d ? p0 : s1 + p0) - s0 + a1 * l, d ? s1 : a1 - s1);
				}
			}
			else {
				float p0 = isHorizontal == 0 ? m_Padding.left : m_Padding.top;
				float p1 = isHorizontal == 0 ? m_Padding.right : m_Padding.bottom;
				var height = (isHorizontal == 1 ? rect.height : rect.width) - p0 - p1;
				for (int i = 0; i < base.rectChildren.Count; i++) {
					base.SetChildAlongAxis(base.rectChildren[i], axis, p0, height);
				}
			}
		}

		/// <summary>
		/// Gets the length of the first child element along the axis.
		/// </summary>
		/// <param name="groupSize">The size of the layout group.</param>
		/// <returns></returns>
		protected abstract float GetDockElementSize(Vector2 groupSize);
	}
}
