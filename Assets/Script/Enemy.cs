using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;  // Maximum health
    private int currentHealth;   // Current health
    public float speed = 2.0f;   // Reduced speed for more manageable gameplay
    private Transform player;    // Reference to the player's transform

    private void Start()
    {
        currentHealth = maxHealth; // Initialize health
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag
    }

    void FixedUpdate()
    {
        MoveTowardsPlayer();  // Call the function to move towards the player each frame
    }

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            // Move each frame towards the player's position using Rigidbody2D for smooth kinematic movement
            Vector3 newPosition = Vector3.MoveTowards(transform.position, player.position, speed * Time.fixedDeltaTime);
            GetComponent<Rigidbody2D>().MovePosition(newPosition);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;  // Reduce health by the damage amount

        if (currentHealth <= 0)
        {
            Die();  // Call Die function if health is zero or less
        }
        else
        {
            SoundPlayer.GetInstance().PlayHitAudio();  // Play hit sound only if enemy did not die
        }
    }

    private void Die()
    {
        SoundPlayer.GetInstance().PlayDeathAudio();  // Play death sound
        Destroy(gameObject);                         // Destroy enemy game object
    }
}