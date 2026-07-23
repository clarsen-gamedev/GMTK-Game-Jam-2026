// Name: TimerFeedbackUI.cs
// Author: Connor Larsen
// Date: 07/23/2026
// Description: Listens for time changes, updates a temporary text element directly underneath the main timer, flashes the appropriate color, and then smoothly fades out and dissappears

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.PackageManager;

public class TimerFeedbackUI : MonoBehaviour
{
    #region Variables
    public static TimerFeedbackUI Instance { get; private set; }

    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI feedbackText;

    [Header("Animation Settings")]
    [SerializeField] private float popInDuration = 0.15f;
    [SerializeField] private float displayDuration = 0.8f;
    [SerializeField] private float fadeDuration = 0.4f;

    [Header("Scale Settings")]
    [SerializeField] private Vector3 startScale = new Vector3(0.3f, 0.3f, 1f);
    [SerializeField] private Vector3 popScale = new Vector3(1.3f, 1.3f, 1f);
    [SerializeField] private Vector3 normalScale = new Vector3(1f, 1f, 1f);

    [Header("Colors")]
    [SerializeField] private Color gainColor = Color.green;
    [SerializeField] private Color lossColor = Color.red;

    private Coroutine activePopRoutine;
    #endregion

    #region Functions
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Hide the feedback text at the start
        if (feedbackText != null )
        {
            SetTextAlpha(0f);   // Make text invisible
            feedbackText.transform.localScale = startScale;
        }
    }

    // Displays floating text showing time added or lost
    public void ShowFeedback(float amount)
    {
        if (feedbackText == null || amount == 0f) return;

        // Stop existing feedback animation if a new one is triggered quickly
        if (activePopRoutine != null)
        {
            StopCoroutine(activePopRoutine);
        }

        if (amount > 0)
        {
            feedbackText.text = $"+{amount:F0}s";
            feedbackText.color = gainColor;
        }
        else
        {
            feedbackText.text = $"{amount:F0}s";
            feedbackText.color = lossColor;
        }

        activePopRoutine = StartCoroutine(AnimateFeedback());
    }

    private IEnumerator AnimateFeedback()
    {
        float elapsed = 0f;
        Color initialColor = feedbackText.color;

        #region Fast Fade In & Pop/Scale Up
        while (elapsed < popInDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = elapsed / popInDuration;
            SetTextAlpha(Mathf.Lerp(0f, 1f, progress));
            feedbackText.transform.localScale = Vector3.Lerp(startScale, popScale, progress);
            yield return null;
        }

        // Settle back to normal scale
        elapsed = 0f;
        float settleDuration = 0.1f;
        while (elapsed < settleDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            feedbackText.transform.localScale = Vector3.Lerp(popScale, normalScale, elapsed / settleDuration);
            yield return null;
        }

        // Ensure scale and alpha are set
        feedbackText.transform.localScale = normalScale;
        SetTextAlpha(1f);
        #endregion

        #region Hold On Screen
        yield return new WaitForSecondsRealtime(displayDuration);
        #endregion

        #region Smooth Fade Out
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            SetTextAlpha(alpha);
            yield return null;
        }

        // Reset state for next trigger
        SetTextAlpha(0f);
        feedbackText.transform.localScale = startScale;
        #endregion
    }

    private void SetTextAlpha(float alpha)
    {
        Color color = feedbackText.color;
        color.a = alpha;
        feedbackText.color = color;
    }
    #endregion
}