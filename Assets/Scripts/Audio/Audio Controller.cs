using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [Header("UI and Music")]
    public AudioSource buttonClick;
    public AudioSource MainMenuTheme;
    public AudioSource BackgroundMusic;

    [Header("SFX")]
    public AudioSource playerDeath;
    public AudioSource playerTakeDamage;
    public AudioSource playerLevelUp;
    [Space]
    public AudioSource areaWeapon;
    public AudioSource laserSword;
    public AudioSource rootkitBlaster;
    public AudioSource scannerBlaster;
    [Space]
    public AudioSource enemyDeath;
    [Space]
    public AudioSource itemCollect;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void PlaySound(AudioSource audio)
    {
        audio.Stop();
        audio.Play();
    }

    public void PlayModifiedSound(AudioSource audio)
    {
        audio.pitch = Random.Range(0.7f, 1.2f);
        audio.Stop();
        audio.Play();
    }

    private System.Collections.IEnumerator FadeOutAndStop(AudioSource source, float duration)
    {
        if (source == null || !source.isPlaying)
            yield break;
        float startVolume = source.volume;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }
        source.volume = 0f;
        source.Stop();
        source.volume = startVolume;
    }

    private System.Collections.IEnumerator FadeInAndPlay(AudioSource source, float duration, float targetVolume = 1f)
    {
        if (source == null)
            yield break;
        source.volume = 0f;
        if (!source.isPlaying)
            source.Play();
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, targetVolume, time / duration);
            yield return null;
        }
        source.volume = targetVolume;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Menu")
        {
            StartCoroutine(FadeInAndPlay(MainMenuTheme, 1f));
        }
        if (scene.name == "Game")
        {
            StartCoroutine(FadeInAndPlay(BackgroundMusic, 1f));
        }
    }

    public void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == "Main Menu")
        {
            StartCoroutine(FadeOutAndStop(MainMenuTheme, 1f));
        }
        if (scene.name == "Game")
        {
            StartCoroutine(FadeOutAndStop(BackgroundMusic, 1f));
        }
    }

    
}
