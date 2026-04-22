using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    public float maxHealth = 1;
    public float damage = 1;
    public float moveSpeed = 5f;
    [SerializeField] private GameObject ExpOrb;
    public enum EnemyType { Single, Group }
    public EnemyType enemyType;
    public int minEnemies;
    public int maxEnemies;
    public float groupRadius;

    
    public float pushDuration;
    [SerializeField] private GameObject deathFX;


    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 playerPos;
    private Animator animator;

    private PlayerController player;

    private float currentHealth;

    private float pushCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = PlayerController.Instance;
        currentHealth = maxHealth;
        pushCounter = 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.gameObject.activeSelf)
        {
            if(pushCounter > 0)
            {
                pushCounter -= Time.deltaTime;
                if (moveSpeed > 0)
                {
                    moveSpeed = -moveSpeed;
                }
                if(pushCounter <= 0)
                {
                    moveSpeed = Mathf.Abs(moveSpeed);
                }
            }
            playerPos = player.transform.position;
            moveDirection = (playerPos - rb.position).normalized;

            rb.linearVelocity = moveDirection * moveSpeed;

            animator.SetFloat("MoveX", moveDirection.x);
            animator.SetFloat("MoveY", moveDirection.y);

        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            // if(gameObject != null)
            // {
            //     Destroy(gameObject);
            // }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        DamagePopUpsController.Instance.CreatePopUp(damage, transform.position);

        pushCounter = pushDuration;

        if (currentHealth <= 0)
        {
            Instantiate(ExpOrb, transform.position, Quaternion.identity);
            Destroy(gameObject);
            if (deathFX != null)
            {
                Instantiate(deathFX, transform.position, transform.rotation);
                AudioController.Instance.PlayModifiedSound(AudioController.Instance.enemyDeath);
            }
        }
    }
}
