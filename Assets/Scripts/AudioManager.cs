using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    #region Variables
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Music Tracks")]
    [SerializeField] private AudioClip titleTheme;
    [SerializeField] private AudioClip mainGameTheme;
    [SerializeField] private AudioClip gameOverTheme;

    [Header("UI & Menu Clips")]
    [SerializeField] private AudioClip buttonClickClip;

    [Header("UI & Notification Clips")]
    [SerializeField] private AudioClip spawnRateIncreaseClip;
    [SerializeField] private AudioClip timerGainClip;
    [SerializeField] private AudioClip timerLossClip;
    [SerializeField] private AudioClip defenseSwitchClip;

    [Header("Combat & Gameplay Clips")]
    [SerializeField] private AudioClip playerShootClip;
    [SerializeField] private AudioClip enemyHitClip;
    [SerializeField] private AudioClip enemyDefeatClip;

    private Coroutine musicFadeRoutine;
    #endregion

    #region Functions
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
            return;
        }

        // Set up audio sources if not assigned manually
        if (sfxSource == null) sfxSource = GetComponent<AudioSource>();
        if (musicSource == null)
        {
            // Add a second AudioSource component for the music
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }
        sfxSource.ignoreListenerPause = true;
        musicSource.ignoreListenerPause = true;
    }

    #region Music Player Controls
    public void PlayMusic(AudioClip clip, float fadeDuration = 0.5f)
    {
        if (clip == null || musicSource == null) return;

        // Don't restart song if already playing
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        if (musicFadeRoutine != null) StopCoroutine(musicFadeRoutine);
        musicFadeRoutine = StartCoroutine(CrossfadeMusic(clip, fadeDuration));
    }

    // Direct helper methods for quick scene triggers
    public void PlayTitleTheme() => PlayMusic(titleTheme);
    public void PlayMainGameTheme() => PlayMusic(mainGameTheme);
    public void PlayGameOverTheme() => PlayMusic(gameOverTheme);

    public void StopMusic(float fadeDuration = 0.5f)
    {
        if (musicFadeRoutine != null) StopCoroutine(musicFadeRoutine);
        musicFadeRoutine = StartCoroutine(CrossfadeMusic(null, fadeDuration));
    }

    private IEnumerator CrossfadeMusic(AudioClip newClip, float duration)
    {
        float startVolume = musicSource.volume;

        // Fade out current song
        if (musicSource.isPlaying)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
                yield return null;
            }
        }

        // Change clip
        musicSource.clip = newClip;

        // Fade in new song
        if (newClip != null)
        {
            musicSource.Play();
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                musicSource.volume = Mathf.Lerp(0f, startVolume, elapsed / duration);
                yield return null;
            }
        }
        else
        {
            musicSource.Stop();
        }

        musicSource.volume = startVolume;
    }
    #endregion

    #region Sound Effect Controls
    // Play a single clip without interrupting ongoing sounds
    public void PlayOneShot(AudioClip clip, float volume = 1f)
    {
        if (clip != null && sfxSource != null) sfxSource.PlayOneShot(clip, volume);
    }

    // Menu Sounds
    public void PlayButtonSound() => PlayOneShot(buttonClickClip);

    // Notification Sounds
    public void PlaySpawnRateIncreaseSound() => PlayOneShot(spawnRateIncreaseClip);
    public void PlayTimerGainSound() => PlayOneShot(timerGainClip);
    public void PlayTimerLossSound() => PlayOneShot(timerLossClip);
    public void PlayDefenseSwitchSound() => PlayOneShot(defenseSwitchClip);

    // Combat Sounds
    public void PlayPlayerShootSound() => PlayOneShot(playerShootClip, 0.2F);
    public void PlayEnemyHitSound() => PlayOneShot(enemyHitClip);
    public void PlayEnemyDefeatedSound() => PlayOneShot(enemyDefeatClip);
    #endregion
    #endregion
}