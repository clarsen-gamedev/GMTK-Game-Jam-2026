// Name: Enemy.cs
// Author: Connor Larsen
// Date: 07/23/2026
// Description: Handles target tracking, taking damage from bullets, and adjusting the game manager to alter the countdown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    #region Variables
    public enum TargetType { PLAYER, DEFENSE_POINT, NONE };

    [Header("Behaviour Settings")]
    [SerializeField] private TargetType targetType = TargetType.PLAYER;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float maxHealth = 20f;

    [Header("Time Values")]
    [SerializeField] private float timeAddedOnKill = 3f;
    [SerializeField] private float timeLostOnImpact = 5f;

    private Transform targetTransform;
    private Rigidbody2D rb;
    private float currentHealth;
    #endregion

    #region Functions
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        FindTarget();
    }

    private void FindTarget()
    {
        if (targetType == TargetType.PLAYER)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) targetTransform = playerObj.transform;
        }
        else if (targetType == TargetType.DEFENSE_POINT)
        {
            GameObject defenseObj = GameObject.FindGameObjectWithTag("DefensePoint");
            if (defenseObj != null) targetTransform = defenseObj.transform;
        }
    }

    private void FixedUpdate()
    {
        if (targetTransform == null) return;

        // Move towards designated target
        Vector2 direction = ((Vector2)targetTransform.position - rb.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        // Rotate sprite to face movement direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0f)
        {
            // Enemy defeated, add time to timer
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddTime(timeAddedOnKill);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore bullet triggers
        if (collision.CompareTag("Bullet")) return;

        bool isPlayer = collision.CompareTag("Player");
        bool isDefensePoint = collision.CompareTag("DefensePoint");

        bool validImpact = false;

        // Player-chasers only explode on player
        if (targetType == TargetType.PLAYER && isPlayer)
        {
            validImpact = true;
        }
        // Defense-chasers explode on BOTH Defense Point and Player
        else if (targetType == TargetType.DEFENSE_POINT && (isDefensePoint || isPlayer))
        {
            validImpact = true;
        }

        if (validImpact)
        {
            // Subtract time penalty directly
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddTime(-timeLostOnImpact);
            }

            // Despawn the enemy
            Destroy(gameObject);
        }

        //// Check if enemy reached its designated target
        //bool hitPlayer = targetType == TargetType.PLAYER && collision.CompareTag("Player");
        //bool hitDefense = targetType == TargetType.DEFENSE_POINT && collision.CompareTag("DefensePoint");

        //bool shouldExplode = (targetType == TargetType.PLAYER && hitPlayer) ||
        //                     (targetType == TargetType.DEFENSE_POINT && (hitDefense || hitPlayer));

        //if (shouldExplode)
        //{
        //    if (GameManager.Instance != null)
        //    {
        //        GameManager.Instance.AddTime(-timeLostOnImpact);
        //    }
        //}

        //Destroy(gameObject);

        //if (hitPlayer || hitDefense)
        //{
        //    // Enemy reached targer -> subtract time penalty
        //    if (GameManager.Instance != null)
        //    {
        //        GameManager.Instance.AddTime(-timeLostOnImpact);
        //    }
        //    Destroy(gameObject);
        //}
    }
    #endregion
}