using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PowerupItem : MonoBehaviour
{
    #region Variables
    [SerializeField] private PowerupData data;
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobHeight = 0.1f;
    [SerializeField] private float lifeTime = 10f; // Disappears if not picked up

    private Vector3 startPos;
    #endregion

    #region Functions
    private void Start()
    {
        startPos = transform.position;
        Destroy(gameObject, lifeTime); // Auto-despawn after 10s
    }

    private void Update()
    {
        // Simple floating bob animation
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void Initialize(PowerupData powerupData)
    {
        data = powerupData;
        if (data != null && TryGetComponent<SpriteRenderer>(out var sr))
        {
            sr.sprite = data.icon;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && data != null)
        {
            data.ApplyPowerup(collision.gameObject);
            Destroy(gameObject);
        }
    }
    #endregion
}