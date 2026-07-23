// Name: PlayerController.cs
// Author: Connor Larsen
// Date: 07/22/2026
// Description: Controls the player and their various mechanics

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Public and Serialized Variables
    [SerializeField] private float moveSpeed = 8f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    #endregion

    #region Functions
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
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
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
    #endregion
}