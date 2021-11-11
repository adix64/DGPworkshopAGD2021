using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public float freq = 3f;
    public float phase = 3f;
    public float amplitude = 60f;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        float rotZ = Mathf.Sin(Time.time * freq + phase) * amplitude;
        transform.localRotation = Quaternion.Euler(0, 0, rotZ);
    }
}