using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    private enum BossState { Idle, Move, Attack }
    private BossState currentState;

    public Animator animator;
    public Transform player;
    public SpriteRenderer spriteRenderer;

    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackCooldown = 1f; // Time between attacks
    [SerializeField] private int maxHealth = 250;

    private int currentHealth;
    private bool isDead = false;

    private float attackTimer = 0f; // Timer to control attack cooldown
    private float stateChangeCooldown = 0.5f; // 0.5 seconds cooldown between state changes
    private float lastStateChangeTime = 0;

    private void Awake()
    {
        currentHealth = maxHealth; // Initialize health

        // Find the player's transform by looking for the player tag
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Get the Animator and SpriteRenderer components from this GameObject
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void Start()
    {
        currentState = BossState.Idle;
    }

    private void Update()
    {
        if (Time.time - lastStateChangeTime < stateChangeCooldown)
            return; // Skip the rest of the update if we're within the cooldown period

        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        Debug.Log($"isMoving set to: {currentState == BossState.Move}");
        animator.SetBool("IsMoving", currentState == BossState.Move);

        Debug.Log($"isAttacking set to: {currentState == BossState.Attack}");
        animator.SetBool("IsAttacking", currentState == BossState.Attack);

        HandleMovement(direction, distance);
        HandleStateTransitions(distance);
        HandleAttackTimer();
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;  // Change color to red
        yield return new WaitForSeconds(0.1f);  // Duration of the flash
        spriteRenderer.color = Color.white;  // Revert to original color
    }

    private void HandleMovement(Vector2 direction, float distance)
    {
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        spriteRenderer.flipX = direction.x > 0;

        if (currentState == BossState.Move)
        {
            MoveTowardsPlayer(direction);
        }
    }

    private void HandleStateTransitions(float distance)
    {
        if (distance <= attackRange && attackTimer <= 0 && currentState != BossState.Attack)
        {
            TransitionToState(BossState.Attack);
        }
        else if (distance > attackRange && currentState != BossState.Move)
        {
            TransitionToState(BossState.Move);
        }
        else if (distance > detectionRange && currentState != BossState.Idle)
        {
            TransitionToState(BossState.Idle);
        }
    }

    private void HandleAttackTimer()
    {
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
    }

    private void TransitionToState(BossState newState)
    {
        if (Time.time - lastStateChangeTime < stateChangeCooldown)
            return; // Exit if the state change is still on cooldown

        Debug.Log($"Transitioning from {currentState} to {newState}");

        // Check if already in the intended state
        if (currentState == newState) return;

        // Update the state change timer
        lastStateChangeTime = Time.time;

        // Reset triggers to prevent overlap or conflicts
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsAttacking", false);

        // Set the new state triggers
        switch (newState)
        {
            case BossState.Idle:
                // Nothing to set if Idle
                break;
            case BossState.Move:
                animator.SetBool("IsMoving", true);
                break;
            case BossState.Attack:
                animator.SetBool("IsAttacking", true);
                PerformAttack();
                attackTimer = attackCooldown; // Reset the attack timer
                break;
        }

        currentState = newState;
    }

    private void MoveTowardsPlayer(Vector2 direction)
    {
        transform.position += new Vector3(direction.x, direction.y, 0) * (moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damage, string weaponType)
    {
        if (isDead) return;  

        currentHealth -= damage;
        StartCoroutine(FlashRed());  // Start the FlashRed coroutine
        Debug.Log($"Boss took {damage} damage, current health: {currentHealth}");

        // Check weapon type and play corresponding sound
        if (weaponType == "Scythe")
        {
            SoundPlayer.GetInstance().PlayHitAudio();
        }
        else if (weaponType == "Hammer")
        {
            SoundPlayer.GetInstance().PlayOrbitHitAudio();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Boss is dead!");
        // ADD SOUND EFFECT
        Destroy(gameObject); // This destroys the boss GameObject
    }

    private void PerformAttack()
    {
        Debug.Log("Boss is attacking the player!");
        if (player.GetComponent<Player>() != null)
        {
            player.GetComponent<Player>().TakeDamage(10); 
        }
    }
}