using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType
{
    AddTime,
    DecreaseDifficulty,
    None
}

[CreateAssetMenu(fileName = "NewPowerup", menuName = "ScriptableObjects/Powerup Data")]
public class PowerupData : ScriptableObject
{
    #region Variables
    [Header("Display Info")]
    public string powerupName;
    public Sprite icon;
    public GameObject worldPrefab; // Visual object dropped on the ground

    [Header("Powerup Logic")]
    public PowerupType type;
    public float value;         // Amount (e.g., 30 seconds)

    [Header("Audio")]
    public AudioClip pickupSound;
    #endregion

    #region Functions
    // Modular Execution Method
    public void ApplyPowerup(GameObject player)
    {
        switch (type)
        {
            case PowerupType.AddTime:
                // Add time to the main game countdown timer
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.AddTime(value);
                }

                // Show UI feedback
                if (TimerFeedbackUI.Instance != null)
                {
                    TimerFeedbackUI.Instance.ShowTimerFeedback(value);
                }
                break;

            case PowerupType.DecreaseDifficulty:
                EnemySpawner.DecreaseGlobalSpawnRate();
                break;

                // Future powerups added as new cases here
        }

        // Play pickup sound if assigned
        if (pickupSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayOneShot(pickupSound);
        }
    }
    #endregion
}