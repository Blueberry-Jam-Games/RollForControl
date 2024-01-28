using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
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

    public HPBar healthbar;

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
        Debug.Log($"Weapon hand is {weaponHand}");
        RefreshWand(currentWand);
    }

    private void RefreshWand(int wand)
    {
        Debug.Log("Refreshing Wand");
        if (activeWand != null)
        {
            Debug.Log("Active Wand not null");
            shootingref.bulletSpawnPoint = null;
            activeWand = null;
            wandShootPoint = null;
            GameObject.Destroy(activeWand);
        }

        currentWand = wand;
        activeWand = GameObject.Instantiate(wandPrefabs[currentWand], weaponHand.transform);
        Debug.Log($"Active wand created {activeWand.name}");
        wandShootPoint = GameObject.FindWithTag("ShootPoint");
        Debug.Log($"Wand spawn concerned about null of either {shootingref}, {wandShootPoint}");
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
                float xv = Input.GetAxis("Horizontal");
                float zv = Input.GetAxis("Vertical");
                movement = new Vector3(xv, 0, zv) * speed;
                if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    isGrounded = false;
                }

                if (Mathf.Abs(xv) < Mathf.Epsilon && Mathf.Abs(zv) < Mathf.Epsilon)
                {
                    animator.Play("Idle");
                }
                else if (Mathf.Abs(xv) > Mathf.Abs(zv))
                {
                    if (xv > 0)
                    {
                        animator.Play("Run");
                    }
                    else
                    {
                        // backwards
                        animator.Play("Backpedal");
                    }
                }
                else
                {
                    if (zv > 0)
                    {
                        animator.Play("Left");
                    }
                    else
                    {
                        animator.Play("Right");
                    }
                }
                // if (movement.sqrMagnitude > Mathf.Epsilon)
                // {
                //     animator.Play("Run");
                // }
                // else
                // {
                //     animator.Play("Idle");
                // }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Spin();
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
        healthbar.UpdateHealth(hp, 15.0f);
        if (hp <= 0)
        {
            LoseGame();
        }
    }

    void LoseGame()
    {
        Debug.Log("Dead Inside! Just like the devs :D");
    }

    private bool jumping = false;
    private void Jump()
    {
        // TODO check for permission
        if (jumping)
        {
            return;
        }
        jumping = true;
        StartCoroutine(DoJump());
    }

    private IEnumerator DoJump()
    {
        int characterAtStart = currentcharacter;
        float startTime = Time.time;
        float startY = activeCharacter.transform.position.y;

        float delta;
        while((delta = Time.time - startTime) < 0.75f || activeCharacter.transform.position.y <= startY)
        {
            if (characterAtStart != currentcharacter)
            {
                break;
            }
            // -\left(x-1.5\right)^{2}+2.25
            float yOffset = -Mathf.Pow(4 * delta - 1.5f, 2.0f) + 2.25f;
            Vector3 currentPos = activeCharacter.transform.position;
            activeCharacter.transform.position = new Vector3(currentPos.x, startY + yOffset, currentPos.z);
            yield return null;
        }
        
        Vector3 placement = activeCharacter.transform.position;
        activeCharacter.transform.position = new Vector3(placement.x, startY, placement.z);
        jumping = false;
    }

    private bool spinning;

    private void Spin()
    {
        // TODO check for permission
        if (spinning)
        {
            return;
        }
        spinning = true;
        StartCoroutine(DoSpin());
    }

    private float spinTime = 0.5f;
    private IEnumerator DoSpin()
    {
        int characterAtStart = currentcharacter;
        float startTime = Time.time;
        // float startY = activeCharacter.transform.position.y;
        float startYRotation = activeCharacter.transform.rotation.eulerAngles.y;

        float delta;
        while((delta = Time.time - startTime) < spinTime && activeCharacter.transform.rotation.eulerAngles.y <= startYRotation + 360f)
        {
            if (characterAtStart != currentcharacter)
            {
                break;
            }

            float yOffset = delta / spinTime * 360;

            Vector3 prerotation = activeCharacter.transform.rotation.eulerAngles;
            activeCharacter.transform.rotation = Quaternion.Euler(new Vector3(prerotation.x, startYRotation + yOffset, prerotation.z));
            yield return null;
        }
        
        Vector3 rotation = activeCharacter.transform.rotation.eulerAngles;
        activeCharacter.transform.rotation = Quaternion.Euler(new Vector3(rotation.x, startYRotation, rotation.z));
        spinning = false;
    }
}
