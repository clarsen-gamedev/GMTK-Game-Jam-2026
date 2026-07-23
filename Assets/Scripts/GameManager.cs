// Name: GameManager.cs
// Author: Connor Larsen
// Date: 07/22/2026
// Description: Acts as the single source of truth for the game

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager Instance {  get; private set; }

    [Header("Timer Settings")]
    [SerializeField] private float timeRemaining = 60f;
    [SerializeField] private TextMeshProUGUI timerText;

    private bool isTimerRunning = true;
    #endregion

    #region Functions
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = gameObject.GetComponent<GameManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!isTimerRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            timeRemaining = 0;
            isTimerRunning = false;
            UpdateTimerUI();
            OnTimeExpired();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnTimeExpired()
    {
        Debug.Log("Time's up! Game Over!");
        // Game over screen
    }

    public void AddTime(float amount)
    {
        timeRemaining += amount;
        timeRemaining = Mathf.Max(0f, timeRemaining);
    }
    #endregion
}
