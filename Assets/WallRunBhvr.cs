using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunBhvr : StateMachineBehaviour
{
	public float wallRunSpeedMultiplier = 2f;
	public float angleOffset = 45f;
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Player player = animator.GetComponent<Player>();
		animator.transform.forward = Quaternion.Euler(0, angleOffset, 0) * player.wallRunDir;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Rigidbody rigidbody = animator.GetComponent<Rigidbody>();
		rigidbody.velocity = animator.deltaPosition / Time.deltaTime * wallRunSpeedMultiplier;
		// Implement code that processes and affects root motion
	}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that sets up animation IK (inverse kinematics)
	//}
}
