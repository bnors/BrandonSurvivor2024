using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingWeapon : MonoBehaviour
{
    private int baseDamage = 10;
    private int damage;
    public Transform player;                // Reference to the player's transform
    public float orbitDistance = 1.5f;      // Distance from the player
    public float orbitSpeed = 180.0f;       // Speed of orbiting around the player

    private void Start()
    {
        damage = baseDamage;
        Player.OnLevelUp += AdjustDamage;  // Subscribe to the level-up event
    }

    private void OnDestroy()
    {
        Player.OnLevelUp -= AdjustDamage;  // Unsubscribe to prevent memory leaks
    }

    private void AdjustDamage(int playerLevel)
    {
        // Adjust the damage based on player's level
        damage = baseDamage + (playerLevel * 5);  // Example scaling formula
        Debug.Log($"Orbiting weapon damage adjusted to {damage} for level {playerLevel}");
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log($"Orbiting weapon is dealing {damage} damage to {other.name}");
            other.GetComponent<Enemy>().TakeDamage(damage);  // Apply the scaled damage
            SoundPlayer.GetInstance().PlayOrbitHitAudio();  // Play the orbit weapon hit sound
        }
    }
}
