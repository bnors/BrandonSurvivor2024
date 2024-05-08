using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] float MoveSpeed;
    [SerializeField] GameObject scythePrefab;
    [SerializeField] float scytheTime = 2;
    [SerializeField] Slider xpBar;  // Reference to the UI slider for XP
    [SerializeField] TextMeshProUGUI levelText;  // Reference to the UI text for displaying the level

    float currentScytheTimer;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;

    private int currentXP = 0;
    private int currentLevel = 1;
    private int xpToNextLevel = 100;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateXPUI();  // Initial UI update
    }


    private void Update()
    {
        HandleScytheTimer();
        HandleMovement();

        // Test XP addition with a key press
        if (Input.GetKeyDown(KeyCode.X))
        {
            AddXP(10);  // Adds 10 XP every time 'X' is pressed
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    public void AddXP(int xpAmount)
    {
        currentXP += xpAmount;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
        UpdateXPUI();
    }

    private void LevelUp()
    {
        currentLevel++;
        currentXP -= xpToNextLevel;
        xpToNextLevel += 50;  // Increment needed XP for the next level
        levelText.text = "Level: " + currentLevel;  // Update level display
    }

    private void UpdateXPUI()
    {
        xpBar.value = (float)currentXP / xpToNextLevel;  // Update the XP bar's value
        levelText.text = "Level: " + currentLevel;  // Update the level text
    }

    private void HandleScytheTimer()
    {
        currentScytheTimer -= Time.deltaTime;
        if (currentScytheTimer <= 0)
        {
            SpawnScythes();
            currentScytheTimer = scytheTime;
        }
    }

    private void SpawnScythes()
    {
        for (int i = 0; i < 3; i++)
        {
            Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360f));

            // Pass the appropriate pool name, like "Scythe"
            GameObject scythe = ObjectPool.Instance.GetPooledObject("Scythe");

            if (scythe != null)
            {
                scythe.transform.SetPositionAndRotation(transform.position, rot);
                scythe.SetActive(true);
            }
        }
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", x);
        animator.SetFloat("Vertical", y);
        //Debug.Log($"Horizontal: {x}, Vertical: {y}");

        spriteRenderer.flipX = x > 0;
    }

    private void ApplyMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(x, y) * MoveSpeed;
    }
}