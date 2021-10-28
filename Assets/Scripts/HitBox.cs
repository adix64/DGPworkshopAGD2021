using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public string hurtBox;
    public string side;
    public int damage = 5;

    // Start is called before the first frame update
    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer(hurtBox))
            return;
        var animator = other.GetComponentInParent<Animator>();
        animator.SetInteger("takenDamage", damage);
        animator.Play("takeHit" + side);
    }
}