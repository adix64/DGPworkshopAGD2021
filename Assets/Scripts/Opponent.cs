using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : Fighter
{
    public Transform player;
    public float stoppingDistance = 1f;
    public float attackDistance = 1.2f;
    public bool offensive = false;

    [Range(0f, 1f)]
    public float offensiveRate = 0.2f;

    public enum TargetOffsetType { TARGET_PLAYER, LEFT_OFFSET, RIGHT_OFFSET, RETREAT, NUM_TARGET_OFFSETS };

    private TargetOffsetType offsetType = TargetOffsetType.TARGET_PLAYER;
    private Vector3 targetOffset;

    // Start is called before the first frame update
    private void Start()
    {
        InitCommonComponents();
        StartCoroutine(ChangeOffensiveBehavior());
        StartCoroutine(ChangeTargetOffsetBehavior());
    }

    private IEnumerator ChangeTargetOffsetBehavior()
    {
        yield return new WaitForSeconds(Random.Range(1f, 2.5f));
        offsetType = (TargetOffsetType)Random.RandomRange(0, (int)TargetOffsetType.NUM_TARGET_OFFSETS);
        switch (offsetType)
        {
            case TargetOffsetType.TARGET_PLAYER:
                targetOffset = Vector3.zero;
                break;

            case TargetOffsetType.LEFT_OFFSET:
                targetOffset = -transform.right * 2f;
                break;

            case TargetOffsetType.RIGHT_OFFSET:
                targetOffset = transform.right * 2f;
                break;

            case TargetOffsetType.RETREAT:
                targetOffset = -transform.forward * 2f;
                break;
        }

        yield return StartCoroutine(ChangeTargetOffsetBehavior());
    }

    private IEnumerator ChangeOffensiveBehavior()
    {
        yield return new WaitForSeconds(0.5f);
        offensive = Random.Range(0f, 1f) < offensiveRate;
        yield return StartCoroutine(ChangeOffensiveBehavior());
    }

    private void OnEnable()
    {
        player.GetComponent<Player>().RegisterOpponent(transform);
    }

    // Update is called once per frame
    private void Update()
    {
        if (stateInfo.IsName("VesnicaPomenire"))
            return;
        Vector3 toPlayer = player.position - transform.position;
        float distToPlayer = toPlayer.magnitude;
        if (distToPlayer > stoppingDistance)
            moveDir = (toPlayer + targetOffset).normalized;
        else
            moveDir = Vector3.zero;

        HandleAttack(distToPlayer);
        animator.SetFloat("distToEnemy", toPlayer.magnitude);
        Vector3 toPlayer_xOz = Vector3.ProjectOnPlane(toPlayer.normalized, Vector3.up);
        ApplyRootRotation(toPlayer_xOz.normalized);
        base.UpdateFighter();
    }

    private void HandleAttack(float distToPlayer)
    {
        if (offensive && distToPlayer < attackDistance)
            animator.SetTrigger("Punch");
    }
}