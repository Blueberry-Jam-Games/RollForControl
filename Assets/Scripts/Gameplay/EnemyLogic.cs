using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    private int hp = 3;
    private bool started = false;
    public float speed = 1f;
    public bool passedOnce = false;

    private GameObject waifu;

    [SerializeField]
    private HPBar hpBar;

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
        }
        else if (started)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (!passedOnce && transform.position.x - waifu.transform.position.x < 0)
        {
            passedOnce = true;
            RigidBodyMovement pc = waifu.GetComponent<RigidBodyMovement>();
            pc.TakeDamage();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // If you want to despawn on collision with another object, you can handle it here
        if (collision.gameObject.tag == "PlayerBullet")
        {
            hp--;
            hpBar.UpdateHealth(hp, 3.0f);
        }
        if (hp <= 0)
        {
            Destroy(gameObject);
            LootboxDrop();
        }
    }

    void LootboxDrop()
    {

    }


    void AnimateThis()
    {

    }
}