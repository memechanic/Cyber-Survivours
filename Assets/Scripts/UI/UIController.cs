using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    private PlayerController player;

    [SerializeField] private TMP_Text timerText;
    [Space]
    public BarContoller healthBar;
    private float maxHealth;
    private float currentHealth;
    [Space]
    public BarContoller expirienceBar;
    public TMP_Text levelText;
    private float maxExpirience;
    private float currentExpirience;
    private float currentLevel;
    [Space]
    public TMP_Text coinText;
    private int sessionCoins;
    [Space]
    public GameObject gameOverPanel;
    public GameObject pausePanel;

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

    public void UpdateHealthBar()
    {
        player = PlayerController.Instance;
        maxHealth = player.maxHealth;
        currentHealth = player.currentHealth;
        healthBar.UpdateBarValue(maxHealth, currentHealth);
    }

    public void UpdateExpirienceBar()
    {
        player = PlayerController.Instance;
        maxExpirience = player.maxExpirience;
        currentExpirience = player.currentExpirience;
        expirienceBar.UpdateBarValue(maxExpirience, currentExpirience);
    }

    public void UpdateLevelText()
    {
        player = PlayerController.Instance;
        currentLevel = player.currentLevel;
        levelText.text = $"Level: {currentLevel}";
    }

    public void UpdateCoinText()
    {
        player = PlayerController.Instance;
        sessionCoins = player.sessionCoins;
        coinText.text = $"Coins: {sessionCoins}";
    }

    public void UpdateTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

}
