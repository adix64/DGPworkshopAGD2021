using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private float pitch = 0f;
    private float yaw = 0f;
    public Transform player;
    public Transform target;
    public Vector3 cameraOffset;
    public Vector3 aimingCameraOffset;
    private Player playerCtrl;
    private Animator playerAnimator;
    private Vector3 realCameraOffset;
    private Camera camera;
    private float fov = 60f;
    private Vector3 smoothOpponentPosition;
    public float minPitch = -60f;
    public float maxPitch = 60f;

    // Start is called before the first frame update
    private void Start()
    {
        camera = GetComponent<Camera>();
        playerCtrl = player.GetComponent<Player>();
        playerAnimator = player.GetComponent<Animator>();
        realCameraOffset = cameraOffset;
        smoothOpponentPosition = player.position;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        target.position = player.position;
        if (playerCtrl.inEnemyRange)
        {
            float f = playerCtrl.closestDistanceToOpponent / playerCtrl.maxDistToFaceOpponent;
            smoothOpponentPosition = Vector3.Lerp(smoothOpponentPosition, playerCtrl.opponent.position, Time.deltaTime * 5f);
            target.position = Vector3.Lerp(smoothOpponentPosition, player.position, Mathf.Clamp01(f));
            fov = Mathf.Lerp(30, 60, f);
            realCameraOffset.x = Mathf.Lerp(realCameraOffset.x, 0, Time.deltaTime);
        }
        else
        {
            smoothOpponentPosition = player.position;
            fov = 60f;
            realCameraOffset.x = Mathf.Lerp(realCameraOffset.x, cameraOffset.x, Time.deltaTime);
        }
        camera.fieldOfView = fov;
        transform.position = target.position + transform.TransformDirection(realCameraOffset);

        if (playerAnimator.GetBool("Aiming"))
        {
            transform.position = player.position + transform.TransformDirection(aimingCameraOffset);
        }
    }
}