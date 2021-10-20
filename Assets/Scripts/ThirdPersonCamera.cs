using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private float pitch = 0f;
    private float yaw = 0f;
    public Transform target;
    public Vector3 cameraOffset;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -60f, 60f);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        transform.position = target.position + transform.TransformDirection(cameraOffset);
    }
}