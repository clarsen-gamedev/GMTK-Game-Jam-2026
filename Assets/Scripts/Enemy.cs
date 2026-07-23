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

    [Header("Hit Flash Settings")]
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private Transform targetTransform;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine flashRoutine;
    private float currentHealth;
    #endregion

    #region Functions
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        FindTarget();
    }

    public void SetNewTarget(Transform newTarget)
    {
        targetTransform = newTarget;
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
            // Ask the DefensePointManager for the active target
            if (DefensePointManager.Instance != null)
            {
                targetTransform = DefensePointManager.Instance.GetCurrentDefensePoint();
                DefensePointManager.Instance.RegisterDefenseEnemy(this);
            }

            //GameObject defenseObj = GameObject.FindGameObjectWithTag("DefensePoint");
            //if (defenseObj != null) targetTransform = defenseObj.transform;
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
        else
        {
            // Enemy survived the hit -> Flash red!
            TriggerHitFlash();
        }
    }

    private void TriggerHitFlash()
    {
        if (spriteRenderer == null) return;

        // Stop previous flash coroutine if shot repeatedly in rapid succession
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
        flashRoutine = null;
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
    }

    private void OnDestroy()
    {
        // Clean up registration when enemy is killed or despawns
        if (targetType == TargetType.DEFENSE_POINT && DefensePointManager.Instance != null)
        {
            DefensePointManager.Instance.UnregisterDefenseEnemy(this);
        }
    }
    #endregion
}