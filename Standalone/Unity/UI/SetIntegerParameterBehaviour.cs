using UnityEngine;

namespace Cryville.Common.Unity.UI {
	public class SetIntegerParameterBehaviour : SetParameterBehaviour {
		[SerializeField] int m_value;
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			animator.SetInteger(m_name, m_value);
		}
	}
}
