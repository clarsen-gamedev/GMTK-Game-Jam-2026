// Name: PlayerAnimation.cs
// Author: Connor Larsen
// Date: 07/23/2026
// Description: Keeps track of movement inputs, velocity and facing directions to play correct sprite animations

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    #region Variables
    [SerializeField] private Animator animator;

    private Vector2 moveInput;
    private Vector2 attackInput;
    private Vector2 lastFacingDirection = Vector2.down;     // Default facing down
    #endregion

    #region Functions
    private void Update()
    {
        // Gather movement inputs
        moveInput = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) moveInput.y += 1f;
        if (Input.GetKey(KeyCode.S)) moveInput.y -= 1f;
        if (Input.GetKey(KeyCode.D)) moveInput.x += 1f;
        if (Input.GetKey(KeyCode.A)) moveInput.x -= 1f;

        if (moveInput.sqrMagnitude > 0)
        {
            moveInput.Normalize();
            lastFacingDirection = moveInput;    // Store last facing direction when NOT shooting
        }

        // Gather attack inputs
        attackInput = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow)) attackInput.y += 1f;
        if (Input.GetKey(KeyCode.DownArrow)) attackInput.y -= 1f;
        if (Input.GetKey(KeyCode.RightArrow)) attackInput.x += 1f;
        if (Input.GetKey(KeyCode.LeftArrow)) attackInput.x -= 1f;

        bool isAttacking = attackInput.sqrMagnitude > 0;

        if (isAttacking)
        {
            attackInput.Normalize();
            lastFacingDirection = attackInput;  // Shooting overrides facing direction so player faces their aim direction
        }

        // Update animator parameters
        UpdateAnimatorParameters(isAttacking);
    }

    private void UpdateAnimatorParameters(bool isAttacking)
    {
        if (animator == null) return;

        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        // Feed parameters to the animator controller
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
        animator.SetFloat("LastMoveX", lastFacingDirection.x);
        animator.SetFloat("LastMoveY", lastFacingDirection.y);
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsAttacking", isAttacking);
    }
    #endregion
}