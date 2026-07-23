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
    #endregion

    #region Private Variables
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
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(moveX, moveY);
        moveInput = moveInput.normalized;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
    #endregion
}