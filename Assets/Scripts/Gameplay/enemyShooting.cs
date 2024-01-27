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
        if (!started && transform.position.x - waifu.transform.position.x < 17)
        {
            started = true;
            timer = Random.Range(0f,1f);
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
        if (transform.position.x - waifu.transform.position.x < 17)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = bulletSpawnPoint.right * bulletSpeed;
        }
    }
}
