using UnityEngine;

namespace Cryville.Common.Unity.UI {
	public abstract class SetParameterBehaviour : StateMachineBehaviour {
		[SerializeField] protected string m_name;
		public abstract override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
	}
}
