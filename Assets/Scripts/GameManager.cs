// Name: GameManager.cs
// Author: Connor Larsen
// Date: 07/22/2026
// Description: Acts as the single source of truth for the game

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager Instance {  get; private set; }

    [Header("Timer Settings")]
    [SerializeField] private float timeRemaining = 61f;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Game Over Settings")]
    [SerializeField] private GameObject gameOverPanel;
    public bool IsGameOver => isGameOver;

    private bool isTimerRunning = true;
    private bool isGameOver = false;
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
        #region Game Over Logic
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R)) RestartGame();
            return;
        }
        #endregion

        #region Countdown Timer Logic
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
        #endregion
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
        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddTime(float amount)
    {
        timeRemaining += amount;
        timeRemaining = Mathf.Max(0f, timeRemaining);

        // Call feedback UI script
        if (TimerFeedbackUI.Instance != null && amount != 0f)
        {
            TimerFeedbackUI.Instance.ShowFeedback(amount);
        }
    }
    #endregion
}
