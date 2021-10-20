using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : Fighter
{
    public Transform player;
    public float stoppingDistance = 1f;
    public float attackDistance = 1.2f;

    // Start is called before the first frame update
    private void Start()
    {
        InitCommonComponents();
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
        if (distToPlayer < attackDistance)
            animator.SetTrigger("Punch");
        base.UpdateFighter();
    }
}