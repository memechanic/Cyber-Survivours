using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject deathFX;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 playerPos;
    private Animator animator;

    private PlayerController player;

    public float damage = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = PlayerController.Instance;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.gameObject.activeSelf)
        {

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
            Destroy(gameObject);
            if (deathFX != null)
            {
                Instantiate(deathFX, transform.position, transform.rotation);
            }
        }
    }
}
