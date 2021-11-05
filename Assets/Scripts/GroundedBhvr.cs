using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedBhvr : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distToEnemy = animator.GetFloat("distToEnemy");
        // intre 0 si 2 metri: garda 100% ; intre 2 si 4 metri: garda intre 100% si 0%
        // de exemplu, la 3.5 metri, garda este 25%
        float w = 1 - Mathf.Clamp01((distToEnemy - 2f) / 2f);
        animator.SetLayerWeight(1, animator.GetBool("Aiming") ? 1f : w);
    }

    //clamp(x, a, b) = max(a, min(x, b))
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(1, 0f);//lasa garda cand iese din Grounded
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}