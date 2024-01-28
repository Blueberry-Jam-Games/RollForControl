using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyMovement : MonoBehaviour
{
    public Vector3 jump;
    public float jumpForce = 2.0f;

    public bool isGrounded;
    protected Rigidbody rb;
    public float speed = 5f;
    public float hp = 15;
    Vector3 movement;
    public bool tutorial = false;
    public bool paused = false;

    [Header("Visuals")]
    public int currentcharacter;
    public List<GameObject> characterPrefabs;
    public int currentWand;
    public List<GameObject> wandPrefabs;

    private PlayerShooting shootingref;

    // Things that change when we swap character models
    private GameObject activeCharacter;
    private Animator animator;
    private GameObject weaponHand;
    private GameObject activeWand;
    private GameObject wandShootPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 0.2f, 0.0f);
        shootingref = GetComponent<PlayerShooting>();
        SetCharacter(currentcharacter);
    }

    private void SetCharacter(int character)
    {
        currentcharacter = character;

        if (activeCharacter != null)
        {
            weaponHand = null;
            animator = null;
            shootingref.bulletSpawnPoint = null;
            activeWand = null;
            wandShootPoint = null;
            GameObject.Destroy(activeCharacter);
        }

        activeCharacter = GameObject.Instantiate(characterPrefabs[currentcharacter], transform);
        animator = activeCharacter.GetComponent<Animator>();
        weaponHand = GameObject.FindWithTag("WeaponHand");
        RefreshWand(currentWand);
    }

    private void RefreshWand(int wand)
    {
        if (activeWand != null)
        {
            shootingref.bulletSpawnPoint = null;
            activeWand = null;
            wandShootPoint = null;
            GameObject.Destroy(activeWand);
        }

        currentWand = wand;
        activeWand = GameObject.Instantiate(wandPrefabs[currentWand], weaponHand.transform);
        wandShootPoint = GameObject.FindWithTag("ShootPoint");
        Debug.Log($"Looking for null between {shootingref}, {wandShootPoint}");
        shootingref.bulletSpawnPoint = wandShootPoint.transform;
    }

    private void Update()
    {
        // do the move thing tmrw
        if (!paused)
        {
            if (tutorial)
            {
                float xdir = Input.GetAxis("Horizontal");
                if (xdir < 0)
                {
                    xdir = 0;
                }
                movement = new Vector3(xdir, 0, 0) * speed;
            } 
            else
            {
                movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed;
                if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    isGrounded = false;
                }

                if (movement.sqrMagnitude > Mathf.Epsilon)
                {
                    animator.Play("Run");
                }
                else
                {
                    animator.Play("Idle");
                }
            }
        }
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        // If you want to despawn on collision with another object, you can handle it here
        if (collision.gameObject.tag == "EnemyBullet")
        {
            TakeDamage();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = movement;
    }

    public void TakeDamage()
    {
        hp--;
        if (hp <= 0)
        {
            LoseGame();
        }
    }

    void LoseGame()
    {
        Debug.Log("Dead Inside! Just like the devs :D");
    }

    void Jump()
    {

    }

    void Spin()
    {
        
    }

    void AnimateThis()
    {

    }
}
