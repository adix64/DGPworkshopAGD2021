using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Fighter
{
    private List<Transform> opponents;
    public float maxDistToFaceOpponent = 4f;
    public bool inEnemyRange = false;
    public float closestDistanceToOpponent = 0f;
    public float timeSinceChangedOpponent = 0.5f;
    public Transform opponent;
    public Transform weaponTip;
    public Transform weapon;
    private Transform chest;
    private Transform upperChest;
    private Transform head;
    private Transform rightHand;
    private bool aiming = false;
    public GameObject projectilePrefab;
    public ProjectilePool projectilePool;

    // Start is called before the first frame update
    private void Start()
    {
        InitCommonComponents();
        chest = animator.GetBoneTransform(HumanBodyBones.Chest);
        upperChest = animator.GetBoneTransform(HumanBodyBones.UpperChest);
        rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
        head = animator.GetBoneTransform(HumanBodyBones.Head);
        opponents = new List<Transform>();
    }

    public void RegisterOpponent(Transform opp)
    {
        opponents.Add(opp);
    }

    private void CheckAlive()
    {
        if (animator.GetInteger("HP") <= 0)
            SceneManager.LoadScene("Intro");
    }

    // Update is called once per frame
    private void Update()
    {
        CheckAlive();
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        GetMoveDirection();

        OrientPlayerForward();
        base.UpdateFighter();
        HandleJump();
        ShooterUpdate();
        HandleAttack();

        if (transform.position.y < minPossibleY)
            transform.position = initPos;
    }

    private void LateUpdate()
    {
        if (!aiming)
            return;

        CopyRightHandTransformOnWeapon();
        Quaternion alignWeaponToCamera = Quaternion.FromToRotation(weaponTip.forward,
                                                                   camera.forward);

        alignWeaponToCamera.ToAngleAxis(out float angle, out Vector3 axis);
        angle *= 0.5f;//jumatate din rotatie
        alignWeaponToCamera = Quaternion.AngleAxis(angle, axis);
        chest.rotation = alignWeaponToCamera * chest.rotation;
        upperChest.rotation = alignWeaponToCamera * upperChest.rotation;
        CopyRightHandTransformOnWeapon();

        head.rotation = camera.rotation;
    }

    private void CopyRightHandTransformOnWeapon()
    {
        weapon.position = rightHand.position;
        weapon.rotation = rightHand.rotation;
    }

    private void ShooterUpdate()
    {
        aiming = Input.GetButton("Fire2");
        animator.SetBool("Aiming", aiming);
        weapon.gameObject.SetActive(aiming);
        if (aiming && Input.GetButtonDown("Fire1"))
        {//lanseaza proiectil
            projectilePool.SpawnProjectile(weaponTip.position, weaponTip.rotation);
        }
    }

    private void OrientPlayerForward()
    {
        Vector3 lookDir = moveDir;

        inEnemyRange = false;
        opponent = null;
        float closestDistance = float.PositiveInfinity;
        for (int i = 0; i < opponents.Count; i++)
        {
            Vector3 toOpponent_i = opponents[i].position - transform.position;
            float distToOpponent_i = toOpponent_i.magnitude;
            if (distToOpponent_i < closestDistance &&
                distToOpponent_i < maxDistToFaceOpponent &&//in combat range
                Vector3.Dot(transform.forward, toOpponent_i.normalized) > 0f) // opponent e in fata lui player
            {
                closestDistance = distToOpponent_i;
                lookDir = toOpponent_i.normalized;
                inEnemyRange = true;
                opponent = opponents[i];
            }
        }

        closestDistanceToOpponent = closestDistance;
        animator.SetFloat("distToEnemy", closestDistance);
        lookDir = Vector3.ProjectOnPlane(lookDir, Vector3.up);

        if (aiming)
        {
            lookDir = Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up);
        }
        ApplyRootRotation(lookDir.normalized);
    }

    private void HandleAttack()
    {
        if (!aiming && Input.GetButtonDown("Fire1"))
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