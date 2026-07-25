// Name: ObjectivePointer.cs
// Author: Connor Larsen
// Date: 07/25/2026
// Description: Controls the arrow which points to the currently active defense point when the DP is off screen

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivePointer : MonoBehaviour
{
    #region Variables
    [Header("Settings")]
    [SerializeField] private float distanceFromPlayer = 1.2f;
    [SerializeField] private float viewMargin = 0.05f;

    private SpriteRenderer arrowSpriteRenderer;
    private Camera mainCamera;
    private Transform playerTransform;
    #endregion

    #region Functions
    private void Awake()
    {
        arrowSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        mainCamera = Camera.main;

        // Assume this script is on a child object of the player
        if (transform.parent != null) playerTransform = transform.parent;
    }

    private void Update()
    {
        if (DefensePointManager.Instance == null) return;

        Transform activePoint = DefensePointManager.Instance.GetCurrentDefensePoint();

        // If no active defense point, hide arrow
        if (activePoint == null)
        {
            SetArrowVisible(false);
            return;
        }

        // Check if the active defense point is on-screen
        bool isOnScreen = IsPositionOnScreen(activePoint.position);

        if (isOnScreen) SetArrowVisible(false); // Hide arrow when DP is visible on camera
        else
        {
            SetArrowVisible(true);  // Show arrow and point towards active DP
            UpdatePointer(activePoint.position);
        }
    }

    private bool IsPositionOnScreen(Vector3 targetWorldPosition)
    {
        if (mainCamera == null) mainCamera = Camera.main;

        // Convert world position to Viewport coordinates
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(targetWorldPosition);

        // Check if within screen limits (with margin buffer)
        bool inX = viewportPos.x >= (0f + viewMargin) && viewportPos.x <= (1f - viewMargin);
        bool inY = viewportPos.y >= (0f + viewMargin) && viewportPos.y <= (1f - viewMargin);
        bool inFrontOfCam = viewportPos.z > 0f;

        return inX && inY && inFrontOfCam;
    }

    private void UpdatePointer(Vector3 targetWorldPosition)
    {
        Vector3 playerPos = playerTransform != null ? playerTransform.position : transform.parent.position;
        Vector3 direction = (targetWorldPosition - playerPos).normalized;

        // Calculate rotation angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Position the arrow orbiting at a fixed distance around the player
        transform.position = playerPos + (direction * distanceFromPlayer);
    }

    private void SetArrowVisible(bool visible)
    {
        if (arrowSpriteRenderer != null && arrowSpriteRenderer.enabled != visible)
        {
            arrowSpriteRenderer.enabled = visible;
        }
    }
    #endregion
}