using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHitBhvr : StateMachineBehaviour
{
	public AudioClip[] takeHitAudioClip;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int takenDamage = animator.GetInteger("takenDamage");
        int newHP = animator.GetInteger("HP") - takenDamage;
        animator.SetInteger("HP", newHP);

        if (newHP < 0)
        {
            animator.Play("VesnicaPomenire");
        }
		var audioSource = animator.GetComponent<AudioSource>();
		audioSource.clip =
			takeHitAudioClip[Random.Range(0, takeHitAudioClip.Length)];

		if(audioSource.isPlaying)
			audioSource.Stop();
		audioSource.Play();
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		var skinnedMesh = animator.GetComponentInChildren<SkinnedMeshRenderer>();

		float newBlendshapeWeight = Mathf.Lerp(skinnedMesh.GetBlendShapeWeight(0), 100f, Time.deltaTime * 20f);
		skinnedMesh.SetBlendShapeWeight(0, newBlendshapeWeight);
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		var skinnedMesh = animator.GetComponentInChildren<SkinnedMeshRenderer>();
		skinnedMesh.SetBlendShapeWeight(0, 0);

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