using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    private int hp = 3;
    private bool started = false;
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
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
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // If you want to despawn on collision with another object, you can handle it here
        hp--;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
