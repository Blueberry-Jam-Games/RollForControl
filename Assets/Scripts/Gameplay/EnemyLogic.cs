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
    
    [SerializeField]
    private Animator characterAnimator;

    [SerializeField]
    private CharacterSoundContainer sounds;

    private string[] hitsounds = {"hit1", "hit2"};

    // Start is called before the first frame update
    void Start()
    {
        waifu = GameObject.FindGameObjectWithTag("Waifu");

        RuntimeAnimatorController runtimeController = characterAnimator.runtimeAnimatorController;
        RuntimeAnimatorController newController = Instantiate(runtimeController);
        characterAnimator.runtimeAnimatorController = newController;

        characterAnimator.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseControl.Instance.IsPaused(0))
        {
            if (!started && transform.position.x - waifu.transform.position.x < 17)
            {
                started = true;
                StartCoroutine(RandomAnimationTime());
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
    }

    private IEnumerator RandomAnimationTime()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 2.0f));
        characterAnimator.Play("Run");
    }

    void OnCollisionEnter(Collision collision)
    {
        // If you want to despawn on collision with another object, you can handle it here
        if (collision.gameObject.tag == "PlayerBullet")
        {
            hp--;
            hpBar.UpdateHealth(hp, 3.0f);
            if (hp > 0)
            {
                sounds.PlaySound(hitsounds[Random.Range(0, 2)]);
            }
        }
        if (hp <= 0)
        {
            LootboxDrop();
            sounds.PlaySound("die" + Random.Range(0, 10).ToString());
            StartCoroutine(DestroyLater());
        }
    }

    private IEnumerator DestroyLater()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void LootboxDrop()
    {

    }
}
