using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyMovement : MonoBehaviour
{
    protected Rigidbody rb;
    public float speed = 5f;
    public float hp = 15;
    Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // do the move thing tmrw
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        // If you want to despawn on collision with another object, you can handle it here
        if (collision.gameObject.tag == "EnemyBullet")
        {
            hp--;
        }
        if (hp <= 0)
        {
            LoseGame();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = movement;
    }

    private void LoseGame()
    {

    }
}
