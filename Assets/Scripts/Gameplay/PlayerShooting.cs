using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10f;
    public float cooldown = 0.25f;
    public float timer = 0f;

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        } else if (Input.GetButton("Fire1")) // You can customize the input based on your needs
        {
            Shoot();
            timer = cooldown;
        }
    }

    void Shoot()
    {
        Vector3 spawnpoint = new Vector3(bulletSpawnPoint.position.x, 0.5f, bulletSpawnPoint.position.z);
        GameObject bullet = Instantiate(bulletPrefab, spawnpoint, Quaternion.Euler(Vector3.zero));
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = new Vector3(1.0f, 0.0f, 0.0f) * bulletSpeed;
    }
}
