using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingWeapon : MonoBehaviour
{
    private int baseDamage = 10;
    private int damage;
    public Transform player;
    public float orbitDistance = 1.5f;
    public float orbitSpeed = 180f;

    private void Start()
    {
        // Initialize the damage based on the player's current level at startup
        AdjustDamage(Player.GetInstance().GetCurrentLevel());

        Player.OnLevelUp += AdjustDamage;  // Subscribe to the level-up event
        UpdateDamageText();  // Initialize the damage text on startup
    }

    private void OnDestroy()
    {
        Player.OnLevelUp -= AdjustDamage;  // Unsubscribe from the level-up event
    }

    public int GetDamage()
    { 
        return damage; 
    }

    private void AdjustDamage(int playerLevel)
    {
        // Adjust the damage based on the player's level
        damage = baseDamage + (playerLevel * 5);  // Example scaling formula
        Debug.Log($"Orbiting hammer damage adjusted to {damage} for level {playerLevel}");
        UpdateDamageText();  // Update the UI through the UI manager
    }

    private void UpdateDamageText()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateOrbitingWeaponDamageText(damage);
        }
    }

    void Update()
    {
        if (player != null)
        {
            OrbitAroundPlayer();
        }
    }

    void OrbitAroundPlayer()
    {
        transform.position = player.position + (transform.position - player.position).normalized * orbitDistance;
        transform.RotateAround(player.position, Vector3.forward, orbitSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // This block checks for damage against enemies
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log($"Orbiting hammer is dealing {damage} damage to {enemy.name}");
                enemy.TakeDamage(damage);
            }
        }

        // This block checks for damage against the boss
        if (collision.gameObject.CompareTag("Boss"))
        {
            BossEnemy boss = collision.GetComponent<BossEnemy>();
            if (boss != null)
            {
                Debug.Log($"Hammer is dealing {damage} damage to {boss.name}");
                boss.TakeDamage(damage, "Scythe");
                SoundPlayer.GetInstance().PlayOrbitHitAudio();  // Play the orbit weapon hit sound
            }
        }
    }
    
   
}