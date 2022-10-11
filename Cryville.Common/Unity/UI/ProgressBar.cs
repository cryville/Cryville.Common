using UnityEngine;
using UnityEngine.UI;

namespace Cryville.Common.Unity.UI {
	/// <summary>
	/// A non-interactive <see cref="Slider" /> that has an internal tweening behaviour.
	/// </summary>
	public class ProgressBar : Slider {
		[SerializeField][Range(0f, 1f)]
		[Tooltip("The tweening parameter.")]
		float m_smooth = 0;
		/// <summary>
		/// The tweening parameter.
		/// </summary>
		public float Smooth {
			get { return m_smooth; }
			set { m_smooth = value; }
		}

		[SerializeField]
		[Tooltip("The target value.")]
		float m_targetValue;
		/// <summary>
		/// Gets the current displayed value or sets the target value.
		/// </summary>
		public override float value {
			get { return base.value; }
			set { m_targetValue = value; }
		}

		protected override void Update() {
			base.value = (base.value - m_targetValue) * m_smooth + m_targetValue;
			base.Update();
		}
	}
}
