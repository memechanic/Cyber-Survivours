using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Scene currrentScene;

    public bool isPaused;
    private bool isGameOver = false;
    public float gameTime;

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
        isPaused = false;
    }
    void Start()
    {
        currrentScene = SceneManager.GetActiveScene();
    }
    void Update()
    {
        if (currrentScene.name == "Game")
        {
            gameTime += Time.deltaTime;
            UIController.Instance.UpdateTimer(gameTime);
            if (isPaused) Time.timeScale = 0f;
            else Time.timeScale = 1f;
        }
    }
    public void GameOver()
    {
        isGameOver = true;

        AudioController.Instance.PlaySound(AudioController.Instance.playerDeath);

        SaveManager.Instance.data.coins += PlayerController.Instance.sessionCoins;
        SaveManager.Instance.Save();

        StartCoroutine(ShowGameOverScreen());
    }
    IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(3f);
        isPaused = true;
        UIController.Instance.gameOverPanel.SetActive(isGameOver);
    }

    public void RestartGame()
    {
        isPaused = false;
        SaveGame();
        SceneManager.LoadScene("Game");
    }

    public void LoadCharacterSelect()
    {
        isPaused = false;
        SaveGame();
        SceneManager.LoadScene("Character Select");
    }

    public void LoadMenu()
    {
        isPaused = false;
        SaveGame();
        SceneManager.LoadScene("Main Menu");
    }
    
    public void PauseGame()
    {
        if (!isGameOver)
        {
            isPaused = !isPaused;
            UIController.Instance.pausePanel.SetActive(isPaused);
        }
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }

    public void SaveGame()
    {
        if (PlayerController.Instance != null)
        {
            // Save time
            // Save enemy killed
            // Save Level
            SaveManager.Instance.data.coins += PlayerController.Instance.sessionCoins;
            SaveManager.Instance.Save();
        }        
    }
}
