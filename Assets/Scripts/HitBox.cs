using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public string hurtBox;
    public string side;

    // Start is called before the first frame update
    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer(hurtBox))
            return;
        var animator = other.GetComponentInParent<Animator>();
        animator.Play("takeHit" + side);
    }
}