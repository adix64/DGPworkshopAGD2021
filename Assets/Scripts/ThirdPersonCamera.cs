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
    private Player playerCtrl;
    private Vector3 realCameraOffset;
    private Camera camera;
    private float fov = 60f;
    private Vector3 smoothOpponentPosition;

    // Start is called before the first frame update
    private void Start()
    {
        camera = GetComponent<Camera>();
        playerCtrl = player.GetComponent<Player>();
        realCameraOffset = cameraOffset;
        smoothOpponentPosition = player.position;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -60f, 60f);

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
    }
}