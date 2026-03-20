using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Scene currrentScene;

    private bool isPaused;
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
        StartCoroutine(ShowGameOverScreen());
    }
    IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(1.5f);
        UIController.Instance.gameOverPanel.SetActive(isGameOver);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Character Select");
    }

    public void PauseGame()
    {
        if (!isGameOver)
        {
            isPaused = !isPaused;
            UIController.Instance.pausePanel.SetActive(isPaused);
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
