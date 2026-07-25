// Name: CameraFollow.cs
// Author: Connor Larsen
// Date: 07/22/2026
// Description: Controls the camera which follows the player as they move

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Variables
    [Header("Target Settings")]
    [SerializeField] private Transform target;  // Drag player here

    [Header("Follow Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -15f);

    [Header("Map Boundaries")]
    [SerializeField] private SpriteRenderer mapSpriteRenderer;

    private Camera cam;
    private float minX, maxX, minY, maxY;
    private bool boundsConfigured = false;
    #endregion

    #region Functions
    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam != null ) cam = Camera.main;
    }

    private void Start()
    {
        CalculateBounds();
    }

    private void CalculateBounds()
    {
        if (mapSpriteRenderer == null || cam == null) return;

        // Get total map bounds from the SpriteRenderer
        Bounds mapBounds = mapSpriteRenderer.bounds;

        // Calculate the vertical and horizontal extent of the camera
        float camVertExtent = cam.orthographicSize;
        float camHorizExtent = cam.orthographicSize * cam.aspect;

        // Clamp camera center so the view edges never cross the map edges
        minX = mapBounds.min.x + camHorizExtent;
        maxX = mapBounds.max.x - camHorizExtent;
        minY = mapBounds.min.y + camVertExtent;
        maxY = mapBounds.max.y - camVertExtent;

        // Handle edge case if map is smaller than the camera view, lock to map center
        if (minX > maxX) minX = maxX = mapBounds.center.x;
        if (minY > maxY) minY = maxY = mapBounds.center.y;

        boundsConfigured = true;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Clamp desired target position inside the map boundary limits
        if (boundsConfigured)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed + Time.deltaTime);
        transform.position = smoothedPosition;
    }
    #endregion
}