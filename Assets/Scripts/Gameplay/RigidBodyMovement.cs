using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class RigidBodyMovement : MonoBehaviour
{
    public bool isGrounded;
    protected Rigidbody rb;
    public float speed = 5f;
    public float hp = 15;
    private float maxHealth;

    Vector3 movement;
    public bool tutorial = false;

    [Header("Visuals")]
    public int currentcharacter;
    public List<GameObject> characterPrefabs;
    public int currentWand;
    public List<GameObject> wandPrefabs;

    public HPBar healthbar;

    [SerializeField]
    private CharacterSoundContainer sounds;

    private PlayerShooting shootingref;

    // Things that change when we swap character models
    private GameObject activeCharacter;
    private Animator animator;
    private GameObject weaponHand;
    private GameObject activeWand;
    private GameObject wandShootPoint;

    private string[] spawnSounds = { "catboyspawn", "maidspawn", "maidspawn", "foxspawn" };
    private string prefix { get => currentcharacter == 0 ? "catboy" : "waifu"; }
    private string[] hitSounds = { "hit1", "hit2" };

    private void Start()
    {
        maxHealth = hp;
        rb = GetComponent<Rigidbody>();
        shootingref = GetComponent<PlayerShooting>();
        SetCharacter(currentcharacter);
    }

    public void SetCharacterAndWand(int newCharacter, int newWand)
    {
        // currentcharacter = newCharacter;
        SetCharacter(newCharacter);
        StartCoroutine(FixWandNextFrame(newWand));
    }

    private IEnumerator FixWandNextFrame(int newWand)
    {
        yield return null;
        RefreshWand(newWand);
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

        // make a unique runtime instance of the animator
        RuntimeAnimatorController runtimeController = animator.runtimeAnimatorController;
        RuntimeAnimatorController newController = Instantiate(runtimeController);
        animator.runtimeAnimatorController = newController;

        weaponHand = GameObject.FindWithTag("WeaponHand");
        Debug.Log($"Weapon hand is {weaponHand}");

        shootingref.character = currentcharacter;

        sounds.PlaySound(spawnSounds[currentcharacter]);

        RefreshWand(currentWand);
    }

    private void RefreshWand(int wand)
    {
        Debug.Log($"Refreshing Wand {wand}");
        if (activeWand != null)
        {
            Debug.Log("Active Wand not null, destroying");
            shootingref.bulletSpawnPoint = null;
            wandShootPoint = null;
            GameObject.Destroy(activeWand);
        }

        currentWand = wand;

        if (weaponHand == null)
        {
            weaponHand = GameObject.FindWithTag("WeaponHand");
        }

        Debug.Log($"Weapon hand is {weaponHand.name}");
        activeWand = GameObject.Instantiate(wandPrefabs[currentWand], weaponHand.transform);
        Debug.Log($"Active wand created {activeWand.name}");
        wandShootPoint = GameObject.FindWithTag("ShootPoint");
        Debug.Log($"Wand spawn concerned about null of either {shootingref}, {wandShootPoint}");
        shootingref.bulletSpawnPoint = wandShootPoint.transform;
        shootingref.wand = currentWand;
    }

    private void Update()
    {
        // do the move thing tmrw
        if (!PauseControl.Instance.IsPaused(0))
        {
            float xv;
            float zv;
            if (!FlowManager.Instance.CheckPermission("Premium Control Set"))
            {
                xv = Input.GetAxis("Horizontal");
                if (xv < 0)
                {
                    xv = 0;
                }
                zv = Input.GetAxis("Vertical");
                if (zv < 0)
                {
                    zv = 0;
                }
                // movement = new Vector3(xdir, 0, ydir) * speed;
            } 
            else
            {
                xv = Input.GetAxis("Horizontal");
                zv = Input.GetAxis("Vertical");
            }

            movement = new Vector3(xv, 0, zv) * speed;
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                isGrounded = false;
            }

            if (jumping || spinning)
            {
                // pass
            }
            else if (Mathf.Abs(xv) < Mathf.Epsilon && Mathf.Abs(zv) < Mathf.Epsilon)
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

    private void OnCollisionStay()
    {
        isGrounded = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If you want to despawn on collision with another object, you can handle it here
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            TakeDamage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger enter {other.gameObject.name}");
        if (other.gameObject.CompareTag("WinBox"))
        {
            Debug.Log("Win Level");
            // TODO PASS THE GACHAS HERE WON IN THE LEVEL
            FlowManager.Instance.GameplayWin(new List<LootBoxRoll>());
        }
    }

    private void FixedUpdate()
    {
        if (PauseControl.Instance.IsPaused(0))
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.velocity = movement;
        }
    }

    public void TakeDamage()
    {
        hp--;
        healthbar.UpdateHealth(hp, maxHealth);

        sounds.PlaySound(prefix + hitSounds[Random.Range(0, 2)]);

        if (hp <= 0)
        {
            LoseGame();
        }
    }

    void LoseGame()
    {
        Debug.Log("Dead Inside! Just like the devs :D");
        FlowManager.Instance.GameplayLose();
    }

    private bool jumping = false;
    private void Jump()
    {
        // TODO check for permission
        if (jumping || !FlowManager.Instance.CheckPermission("Jump Button"))
        {
            return;
        }
        jumping = true;
        StartCoroutine(DoJump());
    }

    private IEnumerator DoJump()
    {
        animator.Play("Jump");
        yield return new WaitForSeconds(0.708f);
        jumping = false;
    }

    private bool spinning;

    private void Spin()
    {
        // TODO check for permission
        if (spinning || !FlowManager.Instance.CheckPermission("Spin Button"))
        {
            return;
        }
        spinning = true;
        StartCoroutine(DoSpin());
    }

    private IEnumerator DoSpin()
    {
        animator.Play("Spin");
        yield return new WaitForSeconds(0.985f);
        spinning = false;
    }
}
