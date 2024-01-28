using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10f;
    public float cooldown = 0.25f;
    public float timer = 0f;

    public int wand = 0;
    public int character = 0;

    
    [SerializeField]
    private CharacterSoundContainer sounds;

    private string[] wandSounds = {"wand1", "wand2", "wand3"};

    private string prefix { get => character == 0 ? "catboy" : "waifu"; }
    private string[] atkSounds = {"atk1", "atk2", "atk3"};

    void Update()
    {
        if (!PauseControl.Instance.IsPaused(0))
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (Input.GetButton("Fire1")) // You can customize the input based on your needs
            {
                Shoot();
                timer = cooldown;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                sounds.PlaySound(prefix + atkSounds[Random.Range(0, 3)]);
            }
        }
    }

    void Shoot()
    {
        if (bulletSpawnPoint != null)
        {
            sounds.PlaySound(wandSounds[wand]);
            Vector3 spawnpoint = new Vector3(bulletSpawnPoint.position.x, 0.5f, bulletSpawnPoint.position.z);
            GameObject bullet = Instantiate(bulletPrefab, spawnpoint, Quaternion.Euler(0, 0, 90));
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = new Vector3(1.0f, 0.0f, 0.0f) * bulletSpeed;
        }
    }
}
