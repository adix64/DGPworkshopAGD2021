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

    // Start is called before the first frame update
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        GetMoveDirection();
        // deplasamentul intre 2 frameuri trebuie sa fie proportional cu timpul scurs, a.i. viteza e previzibila:
        //transform.position += moveDir * Time.deltaTime * moveSpeed; //doar pentru non-rigidbody
        HandleJump();
    }

    private void FixedUpdate()
    { //pentru rigidBody nu avem voie sa alteram direct transform.position
        float velY = rigidbody.velocity.y;
        rigidbody.velocity = moveDir * moveSpeed;
        rigidbody.velocity = new Vector3(rigidbody.velocity.x,
                                         velY,
                                         rigidbody.velocity.z);
    }

    private void HandleJump()
    {
        Vector3 jumpDir = (Vector3.up * jumpUpPower + moveDir).normalized;

        if (Input.GetButtonDown("Jump"))
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
    }
}