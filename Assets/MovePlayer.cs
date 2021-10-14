using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public Transform camera;
    private Rigidbody rigidbody;
    public float moveSpeed = 4f;
    private Vector3 moveDir;
    public float jumpUpPower = 4f;
    public float jumpPower = 4f;
    public float rotSpeed = 4f;
    public float groundedThreshold = 0.15f;
    private Animator animator;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        GetMoveDirection();
        ApplyRootRotation();
        // deplasamentul intre 2 frameuri trebuie sa fie proportional cu timpul scurs, a.i. viteza e previzibila:
        //transform.position += moveDir * Time.deltaTime * moveSpeed; //doar pentru non-rigidbody
        HandleJump();
    }

    private void ApplyRootRotation()
    {
        if (moveDir.magnitude < 0.001F) //nu exista miscare
            return;//cod de mai jos discarded, nu rotim cand nu se misca personajul
        Quaternion newRotation = Quaternion.LookRotation(moveDir);
        //LERP(a, b, t) = a * (1-t) + b * t
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotSpeed);
    }

    private void OnAnimatorMove()
    {
        if (!animator.GetBool("Grounded"))
            return;//e in aer, discard cod de mai jos, deci pastreaza inertia si poate sari in fata

        float velY = rigidbody.velocity.y;

        rigidbody.velocity = animator.deltaPosition / Time.deltaTime;

        rigidbody.velocity = new Vector3(rigidbody.velocity.x,
                                         velY,
                                         rigidbody.velocity.z);
    }

    private void FixedUpdate()
    { //pentru rigidBody nu avem voie sa alteram direct transform.position
        //float velY = rigidbody.velocity.y;
        //rigidbody.velocity = moveDir * moveSpeed;
        //rigidbody.velocity = new Vector3(rigidbody.velocity.x,
        //                                 velY,
        //                                 rigidbody.velocity.z);
    }

    private void HandleJump()
    {
        Vector3 jumpDir = (Vector3.up * jumpUpPower + moveDir).normalized;
        Ray ray = new Ray();
        ray.origin = transform.position + Vector3.up * groundedThreshold;
        ray.direction = Vector3.down;

        if (Physics.Raycast(ray, 2f * groundedThreshold))
        {
            animator.SetBool("Grounded", true);
            if (Input.GetButtonDown("Jump"))
                rigidbody.AddForce(jumpDir * jumpPower, ForceMode.VelocityChange);
        }
        else
            animator.SetBool("Grounded", false);
    }

    private void GetMoveDirection()
    {
        //-1 pentru tasta A, 1 pentru tasta D, 0 altfel
        float x = Input.GetAxis("Horizontal"); //pentru gamepad x in [-1,1]
        //-1 pentru tasta S, 1 pentru tasta W, 0 altfel
        float z = Input.GetAxis("Vertical"); //pentru gamepad z in [-1,1]
        moveDir = (camera.right * x + camera.forward * z).normalized;
        moveDir = Vector3.ProjectOnPlane(moveDir, Vector3.up).normalized;

        Vector3 characterSpaceMoveDir = transform.InverseTransformDirection(moveDir);
        animator.SetFloat("Forward", characterSpaceMoveDir.z, 0.2f, Time.deltaTime);
        animator.SetFloat("Right", characterSpaceMoveDir.x, 0.2f, Time.deltaTime);
    }
}