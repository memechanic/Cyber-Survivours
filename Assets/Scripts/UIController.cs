using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    private PlayerController player;
    [SerializeField] private GameObject playerHealthBar;
    [SerializeField] private Gradient HPGradient;
    private RectTransform playerHealthBarRect;
    private Image playerHealthBarImage;
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private TMP_Text timerText;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    
    private float barWidth;

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
        playerHealthBarRect = playerHealthBar.GetComponent<RectTransform>();
        playerHealthBarImage = playerHealthBar.GetComponent<Image>();
        barWidth = playerHealthBarRect.rect.width;
    }

    public void UpdateHealthBar()
    {
        if (player == null)
        {
            player = PlayerController.Instance;
        }
        if (player == null) return;

        float ratio = player.currentHealth / player.maxHealth;
        float offset = barWidth * (1 - ratio);

        playerHealthBarRect.anchoredPosition = new Vector2(-offset, 0);
        playerHealthBarImage.color = HPGradient.Evaluate(ratio);

        playerHealthText.text = $"{player.currentHealth} / {player.maxHealth}";
    }

    public void UpdateTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

}
