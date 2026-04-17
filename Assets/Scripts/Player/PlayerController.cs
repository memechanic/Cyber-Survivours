using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;
using IEnumerator = System.Collections.IEnumerator;

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

    public int currentExpirience;
    public int maxExpirience;
    public int currentLevel = 1;
    public int maxLevel = 30;
    public List<int> expToLevelUp;

    public int sessionCoins;
    
    private int selectedCharacter;
    private Character settings;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private SpriteLibrary slib;

    private bool isImmune;
    [SerializeField] private float immunityDuration;
    [SerializeField] private float immunityTimer;

    private GameManager gameManager;

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
        gameManager = GameManager.Instance;
        sessionCoins = 0;

        selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        settings = characterDB.GetCharacter(selectedCharacter);

        moveSpeed = settings.movementSpeed;
        maxHealth = settings.maxHealth;
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        UIController.Instance.UpdateHealthBar();

        CM_Camera.GetComponent<CinemachineCamera>().Follow = gameObject.transform;

        slib = GetComponent<SpriteLibrary>();
        slib.spriteLibraryAsset = settings.texture;

        int next;
        for (int i = expToLevelUp.Count; i < maxLevel; i++)
        {
            next = Mathf.CeilToInt(expToLevelUp[expToLevelUp.Count - 1] * 1.1f + 10);
            expToLevelUp.Add(next);
        }
        maxExpirience = expToLevelUp[currentLevel - 1];
        UIController.Instance.UpdateExpirienceBar();
        UIController.Instance.UpdateLevelText();
        UIController.Instance.UpdateCoinText();
    }
    // Fixed Update is called once per 0.02 time interval
    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void Update()
    {
        if (immunityTimer > 0)
        {
            immunityTimer -= Time.deltaTime;
        }
        else
        {
            isImmune = false;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (gameManager.isPaused) { return; }

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
        if (!isImmune)
        {
            isImmune = true;
            immunityTimer = immunityDuration;

            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            UIController.Instance.UpdateHealthBar();

            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);
                GameManager.Instance.GameOver();
            }
            GetComponent<SpriteRenderer>().color = Color.red;
            if (gameObject.activeSelf)
            {
                StartCoroutine(ResetColor());
            }
        }
    }

    private IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(immunityDuration);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void GetExp(int exp)
    {
        currentExpirience += exp;
        if (currentExpirience >= maxExpirience)
        {
            currentLevel += 1;
            currentExpirience %= maxExpirience;
            if (currentLevel <= 30)
            {
                maxExpirience = expToLevelUp[currentLevel - 1];
            }
            else
            {
                maxExpirience = expToLevelUp[-1];
            }
            UIController.Instance.UpdateLevelText();
        }
        UIController.Instance.UpdateExpirienceBar();
    }

    public void GetHeal(float healPercent)
    {
        currentHealth += healPercent * maxHealth / 100;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UIController.Instance.UpdateHealthBar();
    }

    public void GetCoin(int amount)
    {
        sessionCoins += amount;
        UIController.Instance.UpdateCoinText();
    }

    public void ApplyItemEffect(ItemData itemData)
    {
        switch (itemData.effectType)
        {
            case ItemData.EffectType.Exp:
                GetExp((int)itemData.value);
                break;
            case ItemData.EffectType.Heal:
                GetHeal(itemData.value);
                break;
            case ItemData.EffectType.Coin:
                GetCoin((int)itemData.value);
                break;
        }
    }
}
