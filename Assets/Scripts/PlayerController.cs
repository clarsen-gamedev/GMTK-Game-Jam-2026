// Name: PlayerController.cs
// Author: Connor Larsen
// Date: 07/22/2026
// Description: Controls the player and their various mechanics

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Player Variables")]
    [SerializeField] private float moveSpeed = 8f;

    [Header("Map Boundaries")]
    [SerializeField] private SpriteRenderer mapSpriteRenderer;
    [SerializeField] private float padding = 0.5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 minBounds;
    private Vector2 maxBounds;
    #endregion

    #region Functions
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    private void Start()
    {
        // Calculate map boundaries once at start based on the SpriteRenderer bounds
        if (mapSpriteRenderer != null)
        {
            Bounds mapBounds = mapSpriteRenderer.bounds;

            minBounds = mapBounds.min;
            maxBounds = mapBounds.max;
        }
    }

    private void Update()
    {
        if(PauseManager.IsPaused)
        {
            moveInput = Vector2.zero;
            return;
        }

        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W)) moveY += 1f;
        if (Input.GetKey(KeyCode.S)) moveY -= 1f;
        if (Input.GetKey(KeyCode.A)) moveX -= 1f;
        if (Input.GetKey(KeyCode.D)) moveX += 1f;

        moveInput = new Vector2(moveX, moveY).normalized;
    }

    private void FixedUpdate()
    {
        // Calculate raw target position
        Vector2 targetPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

        // Clamp target position inside the map bounds (with padding)
        if (mapSpriteRenderer != null)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x + padding, maxBounds.x - padding);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y + padding, maxBounds.y - padding);
        }

        rb.MovePosition(targetPosition);
    }
    #endregion
}