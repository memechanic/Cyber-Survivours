using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    private PlayerController player;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private TMP_Text timerText;
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

    public void UpdateHealthSlider()
    {
        if (player == null)
        {
            player = PlayerController.Instance;
        }
        if (player == null) return;
        playerHealthSlider.maxValue = player.maxHealth;
        playerHealthSlider.value = player.currentHealth;
        playerHealthText.text = $"{player.currentHealth} / {player.maxHealth}";
    }

    public void UpdateTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

}
