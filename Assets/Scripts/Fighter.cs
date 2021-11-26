using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public Transform camera;
    public float moveSpeed = 4f;
    public float jumpUpPower = 4f;
    public float jumpPower = 4f;
    public float rotSpeed = 4f;
    public float groundedThreshold = 0.15f;
    public float minPossibleY = -50f;

    protected Vector3 initPos;
    protected Rigidbody rigidbody;
    protected Vector3 moveDir;
    protected Animator animator;
    protected CapsuleCollider capsule;
    protected bool grounded = true;
    protected AnimatorStateInfo stateInfo;

    // Start is called before the first frame update
    protected void InitCommonComponents()
    {
        animator = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();
        initPos = transform.position;
    }

    // Update is called once per frame
    protected void UpdateFighter()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        CheckIfGrounded();
        SetAnimatorMoveParams();

        if (transform.position.y < minPossibleY)
            transform.position = initPos;
    }

    protected void ApplyRootRotation(Vector3 lookDir)
    {
        if (lookDir.magnitude < 0.001F || stateInfo.IsTag("punch") || stateInfo.IsName("Roll")) //nu exista miscare
            return;//cod de mai jos discarded, nu rotim cand nu se misca personajul
        Quaternion newRotation = Quaternion.LookRotation(lookDir);
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

    protected void CheckIfGrounded()
    {
        Ray ray = new Ray();
        Vector3 rayOriginBase = transform.position + Vector3.up * groundedThreshold;
        ray.direction = Vector3.down;
        //aruncam 9 raze in jos, deplasate de la rayOriginBase, in planul xOz,...
        //... una din centrul(nedeplasata) si 8 de pe conturul capsulei vazuta de sus (cerc)
        grounded = false;
        for (float offsetX = -1f; offsetX <= 1f; offsetX += 1f)
        {
            for (float offsetZ = -1f; offsetZ <= 1f; offsetZ += 1f)
            {
                ray.origin = rayOriginBase + new Vector3(offsetX, 0f, offsetZ).normalized * capsule.radius;
                LayerMask layerMask = ~LayerMask.NameToLayer("Default");
                if (Physics.Raycast(ray, 2f * groundedThreshold, layerMask, QueryTriggerInteraction.Ignore))
                {//exista pamant sub picioare
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * 2f * groundedThreshold, Color.green);
                    grounded = true;
                }
                else
                {//e in aer
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * 2f * groundedThreshold, Color.red);
                }
            }
        }

        animator.SetBool("Grounded", grounded);
    }

    protected void SetAnimatorMoveParams()
    {
        Vector3 characterSpaceMoveDir = transform.InverseTransformDirection(moveDir) * 1.2f;
        animator.SetFloat("Forward", characterSpaceMoveDir.z, 0.2f, Time.deltaTime);
        animator.SetFloat("Right", characterSpaceMoveDir.x, 0.2f, Time.deltaTime);
    }
}