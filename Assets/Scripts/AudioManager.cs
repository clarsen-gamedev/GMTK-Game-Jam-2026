using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    #region Variables
    public static AudioManager Instance { get; private set; }

    [Header("UI & Menu Clips")]
    [SerializeField] private AudioClip buttonClip;

    [Header("UI & Notification Clips")]
    [SerializeField] private AudioClip spawnRateIncreaseClip;
    [SerializeField] private AudioClip timerGainClip;
    [SerializeField] private AudioClip timerLossClip;
    [SerializeField] private AudioClip defenseSwitchClip;

    [Header("Combat & Gameplay Clips")]
    [SerializeField] private AudioClip playerShootClip;
    [SerializeField] private AudioClip enemyHitClip;
    [SerializeField] private AudioClip enemyDefeatClip;

    private AudioSource audioSource;
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

        audioSource = GetComponent<AudioSource>();
        audioSource.ignoreListenerPause = true;
    }

    // Play a single clip without interrupting ongoing sounds
    public void PlayOneShot(AudioClip clip, float volume = 1f)
    {
        if (clip != null && audioSource != null) audioSource.PlayOneShot(clip, volume);
    }

    // Menu Sounds
    public void PlayButtonSound() => PlayOneShot(buttonClip);

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
}