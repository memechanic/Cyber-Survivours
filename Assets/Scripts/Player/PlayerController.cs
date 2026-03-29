using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("PLayer Configuration")]
    [SerializeField] private CharacterDataBase characterDB;

    [Header("Utility")]
    [SerializeField] private GameObject CM_Camera;
    [SerializeField] private float moveSpeed;

    public float maxHealth;
    public float currentHealth;


    private int selectedCharacter;
    private Character settings;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private SpriteLibrary sla;



    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        settings = characterDB.GetCharacter(selectedCharacter);


        moveSpeed = settings.movementSpeed;
        maxHealth = settings.maxHealth;
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        UIController.Instance.UpdateHealthBar();

        CM_Camera.GetComponent<CinemachineCamera>().Follow = gameObject.transform;

        sla = GetComponent<SpriteLibrary>();
        sla.spriteLibraryAsset = settings.texture;
    
    }
    // Fixed Update is called once per 0.02 time interval
    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {

        moveInput = context.ReadValue<Vector2>();

        if (context.performed)
        {
            animator.SetBool("IsWalking", true);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
        
        if (context.canceled) animator.SetBool("IsWalking", false);

        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UIController.Instance.UpdateHealthBar();

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
        }
    }
    
}
