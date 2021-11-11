using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    private Projectile[] allProjectiles;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        allProjectiles = new Projectile[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            allProjectiles[i] = transform.GetChild(i).GetComponent<Projectile>();
    }

    // Update is called once per frame
    public void SpawnProjectile(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < transform.childCount; i++)
            if (!allProjectiles[i].gameObject.activeInHierarchy) //daca e disabled, e liber
            {
                allProjectiles[i].gameObject.SetActive(true);
                allProjectiles[i].transform.position = position;
                allProjectiles[i].transform.rotation = rotation;
                allProjectiles[i].lifeTime = 0f;
                break;
            }
    }
}