using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{
    // Start is called before the first frame update
    private void Start()
    {
        InitCommonComponents();
    }

    // Update is called once per frame
    private void Update()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        GetMoveDirection();

        base.UpdateFighter();
        HandleJump();
        HandleAttack();

        if (transform.position.y < minPossibleY)
            transform.position = initPos;
    }

    private void HandleAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            animator.SetTrigger("Punch");
        }
    }

    private void HandleJump()
    {
        Vector3 jumpDir = (Vector3.up * jumpUpPower + moveDir).normalized;
        if (grounded && Input.GetButtonDown("Jump"))
            rigidbody.AddForce(jumpDir * jumpPower, ForceMode.VelocityChange);
    }

    private void GetMoveDirection()
    {
        //-1 pentru tasta A, 1 pentru tasta D, 0 altfel
        float x = Input.GetAxis("Horizontal"); //pentru gamepad x in [-1,1]
        //-1 pentru tasta S, 1 pentru tasta W, 0 altfel
        float z = Input.GetAxis("Vertical"); //pentru gamepad z in [-1,1]
        moveDir = (camera.right * x + camera.forward * z).normalized;
        moveDir = Vector3.ProjectOnPlane(moveDir, Vector3.up).normalized;

        Debug.DrawLine(transform.position, transform.position + moveDir, Color.white);
    }
}