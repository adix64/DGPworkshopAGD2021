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
        if (side.CompareTo("") == 0)
            animator.Play("takeHit" + (Random.Range(0, 2) == 0 ? "L" : "R"));
        else
            animator.Play("takeHit" + side);
    }
}