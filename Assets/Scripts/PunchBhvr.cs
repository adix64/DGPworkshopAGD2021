using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchBhvr : StateMachineBehaviour
{
    public HumanBodyBones bone;

    //intervalul in care e activ hitBox >>>
    public float hitBoxStartT = 0.035f;

    public float hitBoxStopT = 0.2f;

    //<<<
    public float defaultCapsuleRadius = 0.3f;

    public float radiusMultiplier = 2f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var capsule = animator.GetComponent<CapsuleCollider>();
        capsule.radius = defaultCapsuleRadius * radiusMultiplier;

        float t = stateInfo.normalizedTime;
        var collider = animator.GetBoneTransform(bone).GetComponent<Collider>();
        collider.enabled = t > hitBoxStartT && t < hitBoxStopT;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var capsule = animator.GetComponent<CapsuleCollider>();
        capsule.radius = defaultCapsuleRadius;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var rigidbody = animator.GetComponent<Rigidbody>();

        Vector3 snapToOpponentVelocity = Vector3.zero;

        var player = animator.GetComponent<Player>();
        if (player != null && player.opponent != null)
            snapToOpponentVelocity = (player.opponent.position - animator.transform.position) * 2f;
        float velY = rigidbody.velocity.y;

        rigidbody.velocity = animator.deltaPosition / Time.deltaTime + snapToOpponentVelocity;

        rigidbody.velocity = new Vector3(rigidbody.velocity.x,
                                         velY,
                                         rigidbody.velocity.z);
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}