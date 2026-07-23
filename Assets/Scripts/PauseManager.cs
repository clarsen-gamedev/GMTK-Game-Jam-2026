// Name: PauseManager.cs
// Author: Connor Larsen
// Date: 07/23/2026
// Description: Manages the pause screen and it's various functions

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    #region Variables
    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject confirmResetPanel;

    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "Title Screen";

    public static bool IsPaused { get; private set; } = false;
    #endregion

    #region Functions
    private void Start()
    {
        // Ensure UI panels are hidden and time runs normally at start
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (confirmResetPanel != null) confirmResetPanel.SetActive(false);
        IsPaused = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        // Do NOT allow pausing if the game is already in a Game Over state
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        // Pressing ESC toggles pause mode
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                if (confirmResetPanel != null && confirmResetPanel.activeSelf)
                {
                    CancelResetConfirmation();
                }
                else
                {
                    ResumeGame();
                }
            }
            else
            {
                PauseGame();
            }
        }
    }

    #region Core Pause Methods
    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (confirmResetPanel != null) confirmResetPanel.SetActive(false);
    }
    #endregion

    #region Confirmation Screen Methods
    public void OpenResetConfirmation()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (confirmResetPanel != null) confirmResetPanel.SetActive(true);
    }

    public void CancelResetConfirmation()
    {
        if (confirmResetPanel != null) confirmResetPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true); ;
    }

    public void ConfirmReset()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    #region Navigation Methods
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
    #endregion
    #endregion
}