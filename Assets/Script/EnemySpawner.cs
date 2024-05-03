using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs; // Array of enemy prefabs
    [SerializeField] float spawnRate = 5f;      // Time between each spawn
    [SerializeField] float spawnDistance = 10f; // Distance from the player to spawn enemies
    private Transform playerTransform;          // To store the player's transform

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate); // Wait for the time defined in spawnRate

            for (int i = 0; i < enemyPrefabs.Length; i++)
            {
                Vector3 spawnDirection = Random.insideUnitSphere * spawnDistance; // Get a random direction
                spawnDirection += playerTransform.position; // Position around the player
                spawnDirection.y = 0; // Adjust y to 0 if your game is 3D to keep them on the same plane

                Instantiate(enemyPrefabs[i], spawnDirection, Quaternion.identity);
            }
        }
    }
}
