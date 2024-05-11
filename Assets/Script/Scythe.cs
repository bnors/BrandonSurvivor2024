using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scythe : MonoBehaviour, IPoolable
{
    private int baseDamage = 25;
    private int damage;
    public float lifetime = 2f;

    private void Start()
    {
        // Initialize the damage based on the player's current level at startup
        damage = baseDamage + (Player.GetInstance().GetCurrentLevel() * 10);

        // Subscribe to the level-up event
        Player.OnLevelUp += AdjustDamage;

        UpdateDamageText();  // Initialize the damage text on startup
    }

    private void OnDestroy()
    {
        // Unsubscribe from the level-up event to avoid memory leaks
        Player.OnLevelUp -= AdjustDamage;
    }

    public void Reset()
    {
        lifetime = 2;  // Reset the lifetime but not the damage
    }

    public int GetDamage()
    {
        return damage;
    }

    private void AdjustDamage(int playerLevel)
    {
        // Adjust the damage based on the player's level
        damage = baseDamage + (playerLevel * 10);  // Example scaling formula
        Debug.Log($"Scythe damage adjusted to {damage} for level {playerLevel}");

        // Update the UI to reflect the new damage
        UpdateDamageText();
    }

    private void UpdateDamageText()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScytheDamageText(damage);
        }
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            gameObject.SetActive(false);  // Deactivate the scythe instead of destroying it
        }

        // Move the scythe forward based on its direction
        transform.position += transform.right * 5f * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // This block checks for damage against enemies
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log($"Scythe is dealing {damage} damage to {enemy.name}");
                enemy.TakeDamage(damage);
            }
        }

        // This block checks for damage against the boss
        if (collision.gameObject.CompareTag("Boss"))
        {
            BossEnemy boss = collision.GetComponent<BossEnemy>();
            if (boss != null)
            {
                Debug.Log($"Scythe is dealing {damage} damage to {boss.name}");
                boss.TakeDamage(damage, "Scythe");
            }
        }
    }
}
