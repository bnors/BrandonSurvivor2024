using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float MoveSpeed;
    [SerializeField] GameObject scythePrefab;
    [SerializeField] float scytheTime = 2;
    float currentScytheTimer;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer component

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();  // Initialize the SpriteRenderer component
    }

    private void Update()
    {
        currentScytheTimer -= Time.deltaTime;
        if (currentScytheTimer <= 0)
        {
            // Spawn the Scythe
            for (int i = 0; i < 3; i++)
            {
                Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360f));
                GameObject scythe = ObjectPool.getInstance().getPooledObject();
                scythe.transform.SetPositionAndRotation(transform.position, rot);
                scythe.SetActive(true);
            }
            currentScytheTimer += scytheTime;
        }

        // Handle input and animations
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Update Animator parameters
        animator.SetFloat("Horizontal", x);
        animator.SetFloat("Vertical", y);

        // Animation bug, currently trying to fix
        Debug.Log($"Horizontal: {x}, Vertical: {y}");   

        // Flip sprite based on horizontal movement
        if (x != 0)
        {
            spriteRenderer.flipX = x < 0;
        }
    }

    private void FixedUpdate()
    {
        // Move the player based on input
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(x, y) * MoveSpeed;
    }
}
