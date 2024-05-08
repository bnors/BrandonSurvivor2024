using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public EnemySpawner enemySpawner;
    public GameObject mainUI;
    public GameObject gameOverUI;
    public Player player;  // Use the Player component directly

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Prevent this GameManager from being destroyed
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Assign references dynamically (if they're not already set)
        if (enemySpawner == null)
            enemySpawner = FindObjectOfType<EnemySpawner>();

        if (mainUI == null)
            mainUI = GameObject.Find("MainUI");

        if (gameOverUI == null)
            gameOverUI = GameObject.Find("GameOverUI");

        // Find the Player component directly
        if (player == null)
            player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        // Assign the player and enemySpawner dynamically if not already set
        if (player == null)
            player = FindObjectOfType<Player>();

        if (enemySpawner == null)
            enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    public void ShowGameOverUI()
    {
        if (mainUI != null)
        {
            mainUI.SetActive(false);  // Hide the main UI
        }

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);  // Show the Game Over UI
        }

        Time.timeScale = 0;  // Pause the game
        DisableGameplay();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;  // Resume the game before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the current scene
    }

    public void QuitGame()
    {
        Time.timeScale = 1;  // Ensure time is resumed before quitting
        Application.Quit();  // Quit the application
    }

    private void DisableGameplay()
    {
        // Disable player movement or interactions
        if (player != null)
        {
            // Disable the Player component directly
            player.enabled = false;
        }
        else
        {
            Debug.LogError("Player reference is missing.");
        }

        // Stop the enemy spawner from spawning new enemies
        if (enemySpawner != null)
        {
            enemySpawner.StopSpawning();
        }
        else
        {
            Debug.LogError("EnemySpawner reference is missing.");
        }
    }
}