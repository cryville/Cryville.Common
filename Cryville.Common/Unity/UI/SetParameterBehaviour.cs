using UnityEngine;

namespace Cryville.Common.Unity.UI {
	public abstract class SetParameterBehaviour : StateMachineBehaviour {
		[SerializeField] protected string m_name;
		public override abstract void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
	}
}
