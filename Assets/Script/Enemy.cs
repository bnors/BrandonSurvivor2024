using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string poolName;
    [SerializeField] int attackDamage = 5;
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] Transform fireballSpawnPoint; // Ensure it's set in the Inspector
    [SerializeField] float fireballSpeed = 5f;
    [SerializeField] float fireballCooldown = 3f;
    [SerializeField] int fireballDamage = 8;
    [SerializeField] bool isFireDragon = false;

    public int maxHealth = 100;
    private int currentHealth;
    public float speed = 2.0f;
    private Transform player;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float lastFireballTime;

    public GameObject xpShardPrefab;

    private void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MoveTowardsPlayer();

        if (isFireDragon)
        {
            HandleFireballAttack();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            float horizontal = direction.x;
            float vertical = direction.y;

            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);

            Vector3 newPosition = Vector3.MoveTowards(transform.position, player.position, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);

            if (horizontal != 0)
            {
                spriteRenderer.flipX = horizontal > 0;
            }
        }
    }

    private void HandleFireballAttack()
    {
        // Check if the fireball cooldown has passed and ensure the fireball spawn point is set
        if (Time.time >= lastFireballTime + fireballCooldown && player != null && fireballSpawnPoint != null)
        {
            // Calculate the direction vector towards the player
            Vector2 direction = (player.position - fireballSpawnPoint.position).normalized;

            // Instantiate the fireball at the designated spawn point
            GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
            Rigidbody2D rbFireball = fireball.GetComponent<Rigidbody2D>();
            rbFireball.velocity = direction * fireballSpeed;

            // Optionally set the damage value for the fireball (if required)
            Fireball fireballComponent = fireball.GetComponent<Fireball>();
            if (fireballComponent != null)
            {
                fireballComponent.SetDamage(fireballDamage);
            }

            // Update the last time the fireball was fired
            lastFireballTime = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(attackDamage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            SoundPlayer.GetInstance().PlayHitAudio();
        }
    }

    private void Die()
    {
        Debug.Log($"Enemy {poolName} is dying and will be registered.");

        EnemySpawner.Instance.RegisterEnemyEncounter(poolName);

        DropXPShards();
        SoundPlayer.GetInstance().PlayDeathAudio();
        Destroy(gameObject);
    }

    private void DropXPShards()
    {
        int numShards = Random.Range(1, 4);
        for (int i = 0; i < numShards; i++)
        {
            Vector3 dropPosition = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            Instantiate(xpShardPrefab, dropPosition, Quaternion.identity);
        }
    }
}