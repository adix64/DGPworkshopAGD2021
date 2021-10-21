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

    // Start is called before the first frame update
    private void Start()
    {
        InitCommonComponents();
        StartCoroutine(ChangeOffensiveBehavior());
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
        Vector3 toPlayer = player.position - transform.position;
        float distToPlayer = toPlayer.magnitude;
        if (distToPlayer > stoppingDistance)
            moveDir = toPlayer.normalized;
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