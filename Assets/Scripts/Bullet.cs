// Name: Bullet.cs
// Author: Connor Larsen
// Date: 07/22/2026
// Description: Controls the bullet

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    #region Variables
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private float damage = 10f;

    private Rigidbody2D rb;
    #endregion

    #region Functions
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    private void Start()
    {
        rb.velocity = transform.up * speed;
        Destroy(gameObject, lifeTime);          // Destroy the bullet after a set amount of time
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return; // Do not collide with the player

        // Check if bullet hit an enemy
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
    #endregion
}