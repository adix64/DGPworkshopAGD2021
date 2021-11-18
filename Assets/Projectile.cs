using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 10f;
    public float lifeTime = 0f;
    TrailRenderer trails;

	private void OnEnable()
	{
        lifeTime = 0f;
        trails = GetComponent<TrailRenderer>();
        trails.emitting = true;
        //for (int i = 0; i < trails.positionCount; i++)
        //    trails.SetPosition(i, transform.position);
        
	}
	private void OnDisable()
	{
        trails.emitting = false;
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