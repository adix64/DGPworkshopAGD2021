using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 10f;
    public float lifeTime = 0f;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime > 10f)
            gameObject.SetActive(false);
        transform.position += transform.forward * Time.deltaTime * projectileSpeed;
    }
}