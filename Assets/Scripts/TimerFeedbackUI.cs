// Name: TimerFeedbackUI.cs
// Author: Connor Larsen
// Date: 07/23/2026
// Description: Listens for time changes, updates a temporary text element directly underneath the main timer, flashes the appropriate color, and then smoothly fades out and dissappears

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEditor.PackageManager;

public class TimerFeedbackUI : MonoBehaviour
{
    #region Variables
    public static TimerFeedbackUI Instance { get; private set; }

    [Header("Timer Feedback UI")]
    [SerializeField] private TextMeshProUGUI timerFeedbackText;
    [SerializeField] private Color gainColor = Color.green;
    [SerializeField] private Color lossColor = Color.red;

    [Header("Defense Point Notification UI")]
    [SerializeField] private TextMeshProUGUI defenseNotificationText;

    [Header("Animation Settings")]
    [SerializeField] private float popInDuration = 0.15f;
    [SerializeField] private float displayDuration = 0.8f;
    [SerializeField] private float fadeOutDuration = 0.4f;

    [Header("Scale Settings")]
    [SerializeField] private Vector3 startScale = new Vector3(0.3f, 0.3f, 1f);
    [SerializeField] private Vector3 popScale = new Vector3(1.3f, 1.3f, 1f);
    [SerializeField] private Vector3 normalScale = new Vector3(1f, 1f, 1f);    

    private Coroutine timerRoutine;
    private Coroutine defenseRoutine;
    #endregion

    #region Functions
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Hide all feedback text at the start
        if (timerFeedbackText != null) ResetText(timerFeedbackText);
        if (defenseNotificationText != null) ResetText(defenseNotificationText);
    }

    #region Timer Change Feedback
    // Displays floating text showing time added or lost
    public void ShowFeedback(float amount)
    {
        if (timerFeedbackText == null || amount == 0f) return;

        // Stop existing feedback animation if a new one is triggered quickly
        if (timerRoutine != null) StopCoroutine(timerRoutine);

        if (amount > 0)
        {
            timerFeedbackText.text = $"+{amount:F0}s";
            timerFeedbackText.color = gainColor;
        }
        else
        {
            timerFeedbackText.text = $"{amount:F0}s";
            timerFeedbackText.color = lossColor;
        }

        timerRoutine = StartCoroutine(AnimateText(timerFeedbackText));
    }
    #endregion

    #region Defense Point Notification
    public void ShowDefensePointNotification(string message)
    {
        if (defenseNotificationText == null) return;
        if (defenseRoutine != null) StopCoroutine(defenseRoutine);

        defenseNotificationText.text = message;
        defenseRoutine = StartCoroutine(AnimateText(defenseNotificationText));
    }    
    #endregion

    private IEnumerator AnimateText(TextMeshProUGUI targetText)
    {
        float elapsed = 0f;
        Color initialColor = targetText.color;

        #region Fast Fade In & Pop/Scale Up
        while (elapsed < popInDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = elapsed / popInDuration;

            SetAlpha(targetText, initialColor, Mathf.Lerp(0f, 1f, progress));
            targetText.transform.localScale = Vector3.Lerp(startScale, popScale, progress);
            yield return null;
        }

        // Settle back to normal scale
        elapsed = 0f;
        float settleDuration = 0.1f;
        while (elapsed < settleDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            targetText.transform.localScale = Vector3.Lerp(popScale, normalScale, elapsed / settleDuration);
            yield return null;
        }

        // Ensure scale and alpha are set
        targetText.transform.localScale = normalScale;
        SetAlpha(targetText, initialColor, 1f);
        #endregion

        #region Hold On Screen
        yield return new WaitForSecondsRealtime(displayDuration);
        #endregion

        #region Smooth Fade Out
        elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
            SetAlpha(targetText, initialColor, alpha);
            yield return null;
        }

        // Reset state for next trigger
        ResetText(targetText);
        #endregion
    }

    private void SetAlpha(TextMeshProUGUI targetText, Color baseColor, float alpha)
    {
        Color color = targetText.color;
        color.a = alpha;
        targetText.color = color;
    }

    private void ResetText(TextMeshProUGUI targetText)
    {
        SetAlpha(targetText, targetText.color, 0f);
        targetText.transform.localScale = startScale;
    }
    #endregion
}