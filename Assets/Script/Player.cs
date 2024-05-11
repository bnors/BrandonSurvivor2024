using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.ComponentModel;
using System;

public class Player : MonoBehaviour
{
    public static event Action<int> OnLevelUp; // Level-up event with player's new level as a parameter
    public static event Action OnPlayerDeath; 

    [SerializeField] float MoveSpeed;
    [SerializeField] GameObject scythePrefab;
    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] float scytheTime = 2;
    [SerializeField] Slider xpBar;  // Reference to the UI slider for XP
    [SerializeField] TextMeshProUGUI levelText;  // Reference to the UI text for displaying the level
    [SerializeField] GameObject gameOverUI;
    [SerializeField] AudioClip walkingClip;
    [SerializeField][Range(0f, 1f)] private float walkingVolume = 0.5f; // Adjust this value in the Inspector
    [SerializeField] private int baseScytheCount = 3;  // Starting number of scythes

    private static Player instance;
    private AudioSource walkingAudioSource;
    private bool isMoving;

    public static Player GetInstance() => instance;

    float currentScytheTimer;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;

    private int currentXP = 0;
    private int currentLevel = 1;
    private int xpToNextLevel = 100;
    private int currentHealth;
    private bool isAlive = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
        UpdateHealthUI();
        UpdateXPUI();  // Initial UI update

        // Initialize the walking audio source
        walkingAudioSource = gameObject.AddComponent<AudioSource>();
        walkingAudioSource.clip = walkingClip; // Ensure this clip is set via Inspector
        walkingAudioSource.loop = true; // Loop walking sound

        // Apply the volume
        walkingAudioSource.volume = walkingVolume;
    }

    private void Update()
    {
        HandleScytheTimer();
        HandleMovement();

        // Check if the player is moving and play or stop the walking sound
        isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        if (isMoving && !walkingAudioSource.isPlaying)
        {
            walkingAudioSource.Play();  // Play the walking sound if not already playing
        }
        else if (!isMoving && walkingAudioSource.isPlaying)
        {
            walkingAudioSource.Stop();  // Stop the walking sound if not moving
        }

        // Test XP addition with a key press
        if (Input.GetKeyDown(KeyCode.X))
        {
            AddXP(100);  // Adds 10 XP every time 'X' is pressed
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
        baseScytheCount++;  // Increase the number of scythes spawned
        Debug.Log("Level Up! Increased scythe count to: " + baseScytheCount);

        // Invoke the OnLevelUp event
        OnLevelUp?.Invoke(currentLevel);
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
        for (int i = 0; i < baseScytheCount; i++)  // Spawn scythes based on the current count
        {
            Quaternion rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360f));
            GameObject scythe = ObjectPool.Instance.GetPooledObject("Scythe");  // Ensure this is the correct pool name
            if (scythe != null)
            {
                scythe.transform.SetPositionAndRotation(transform.position, rotation);
                scythe.SetActive(true);
            }
        }
    }

    private void HandleMovementSound()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Check if the player is moving
        bool currentlyMoving = x != 0 || y != 0;

        // If the player is moving and the walking sound isn't playing, start the sound
        if (currentlyMoving && !walkingAudioSource.isPlaying)
        {
            walkingAudioSource.Play();
        }
        // Stop the walking sound if the player stops moving
        else if (!currentlyMoving && walkingAudioSource.isPlaying)
        {
            walkingAudioSource.Stop();
        }
    }


    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", x);
        animator.SetFloat("Vertical", y);

        spriteRenderer.flipX = x > 0;

        // Play walking sound if moving, stop if not moving
        if (x != 0 || y != 0)
        {
            walkingAudioSource.volume = walkingVolume; // Ensure the volume is set
            if (!walkingAudioSource.isPlaying)
            {
                walkingAudioSource.Play();
            }
        }
        else
        {
            if (walkingAudioSource.isPlaying)
            {
                walkingAudioSource.Stop();
            }
        }
    }

    private void ApplyMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(x, y) * MoveSpeed;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive) return;

        currentHealth -= damage;
        UpdateHealthUI();

        // Play the hit sound
        SoundPlayer.GetInstance().PlayPlayerHitAudio();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isAlive = false;
            OnPlayerDeath?.Invoke();  // Invoke the player death event
            ShowGameOverUI();         // Display the game-over screen
            GameManager.Instance.ShowGameOverUI();
        }
    }

    private void UpdateHealthUI()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        healthText.text = $"Health: {currentHealth}/{maxHealth}";
    }

    private void ShowGameOverUI()
    {

        // Activate the Game Over UI canvas
        gameOverUI.SetActive(true);
    }
}