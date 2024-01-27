using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = -10f;
    public float cooldown = 1f;
    public float timer = 0f;

    private GameObject waifu;

    private bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        waifu = GameObject.FindGameObjectWithTag("Waifu");
    }

    // Update is called once per frame
    void Update()
    {
        if (!started && Input.GetButtonDown("Fire1"))
        {
            started = true;
        }
        else if (started)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else // You can customize the input based on your needs
            {
                Shoot();
                timer = cooldown;
            }
        }
    }

    void Shoot()
    {
        Debug.Log(transform.position.x - waifu.transform.position.x);
        if (transform.position.x - waifu.transform.position.x < 17)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = bulletSpawnPoint.right * bulletSpeed;
        }
    }
}
