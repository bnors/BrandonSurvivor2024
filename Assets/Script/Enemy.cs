using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string poolName;  // Set the enemy type's pool name in the Inspector
    [SerializeField] int attackDamage = 5;
    public int maxHealth = 100;  // Maximum health
    private int currentHealth;   // Current health
    public float speed = 2.0f;   // Reduced speed for more manageable gameplay
    private Transform player;    // Reference to the player's transform
    private Animator animator;   // Animator component
    private SpriteRenderer spriteRenderer; // SpriteRenderer component
    private Rigidbody2D rb;      // Rigidbody2D component
    public GameObject xpShardPrefab;

    private void Start()
    {
        currentHealth = maxHealth; // Initialize health
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag
        animator = GetComponent<Animator>(); // Get the Animator component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    void FixedUpdate()
    {
        MoveTowardsPlayer();  // Call the function to move towards the player each frame
    }

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            float horizontal = direction.x;
            float vertical = direction.y;

            // Set animator parameters
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);

            // Move towards the player's position
            Vector3 newPosition = Vector3.MoveTowards(transform.position, player.position, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);

            // Handle sprite flipping based on the x-direction
            if (horizontal != 0)
            {
                spriteRenderer.flipX = horizontal > 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(attackDamage);  // Deal damage to the player
            }
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
        // Debug: Confirm which poolName is being registered
        Debug.Log($"Enemy {poolName} is dying and will be registered.");

        // Register the kill to the enemy spawner using the pool name
        EnemySpawner.Instance.RegisterEnemyEncounter(poolName);

        DropXPShards();
        SoundPlayer.GetInstance().PlayDeathAudio();  // Play death sound
        Destroy(gameObject);                         // Destroy enemy game object
    }

    private void DropXPShards()
    {
        int numShards = Random.Range(1, 4);  // Randomize the number of shards dropped
        for (int i = 0; i < numShards; i++)
        {
            Vector3 dropPosition = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            Instantiate(xpShardPrefab, dropPosition, Quaternion.identity);
        }
    }
}