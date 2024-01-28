using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f; // Lifetime of the bullet in seconds

    private Rigidbody rb;
    private Vector3 initialVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialVelocity = rb.velocity;
        // Destroy the bullet after the specified lifetime
        // Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (!PauseControl.Instance.IsPaused(0))
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If you want to despawn on collision with another object, you can handle it here
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (PauseControl.Instance.IsPaused(0))
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.velocity = initialVelocity;
        }
    }
}
