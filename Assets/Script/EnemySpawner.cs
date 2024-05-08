using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }  // Static property for singleton pattern

    [SerializeField] string[] enemyPoolNames;  // Pool names corresponding to the enemy types
    [SerializeField] float initialSpawnRate = 1f;  // Initial time between each enemy spawn
    [SerializeField] float spawnRateReduction = 0.1f;  // Reduction per wave
    [SerializeField] int initialWaveSize = 1;  // Initial number of enemies per wave per type
    [SerializeField] int incrementPerWave = 1;  // Incremental number of enemies added per wave per type
    [SerializeField] float spawnDistance = 10f;  // Distance from the player to spawn enemies
    [SerializeField] float minimumSpawnRate = 0.5f;  // Minimum spawn rate limit
    [SerializeField] float waveInterval = 10f;  // Time between waves
    private Transform playerTransform;  // To store the player's transform

    private float spawnRate;
    private bool isSpawning = false;
    private Dictionary<string, int> enemyCountPerType;
    private Dictionary<string, bool> encounterFlagPerType;

    private void Awake()
    {
        // Ensure only one instance is created
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize data structures here if needed
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        spawnRate = initialSpawnRate;

        // Initialize each enemy type with the starting wave size
        enemyCountPerType = new Dictionary<string, int>();
        encounterFlagPerType = new Dictionary<string, bool>();

        foreach (string poolName in enemyPoolNames)
        {
            enemyCountPerType[poolName] = initialWaveSize;
            encounterFlagPerType[poolName] = false;  // Initialize encounter flags to false
        }

        StartCoroutine(SpawnWaves());
    }

    public void RegisterEnemyEncounter(string poolName)
    {
        // Verify if the pool name exists in the encounter flag dictionary
        if (encounterFlagPerType.ContainsKey(poolName))
        {
            encounterFlagPerType[poolName] = true;  // Mark this enemy type as encountered
            Debug.Log($"Encounter flag set for {poolName}.");
        }
        else
        {
            Debug.LogWarning($"Pool name {poolName} not found in encounter flags.");
        }
    }

    private IEnumerator SpawnWaves()
    {
        while (true)
        {
            yield return new WaitForSeconds(waveInterval);

            if (!isSpawning)
            {
                isSpawning = true;
                StartCoroutine(SpawnWave());
            }

            // Debug the encounter flag states
            Debug.Log("Wave completed. Checking encounter flags...");
            foreach (string poolName in enemyPoolNames)
            {
                if (encounterFlagPerType[poolName])
                {
                    enemyCountPerType[poolName] += incrementPerWave;
                    encounterFlagPerType[poolName] = false;  // Reset encounter flag for the next wave
                    Debug.Log($"Incremented count for {poolName} to {enemyCountPerType[poolName]}");
                }
                else
                {
                    Debug.Log($"No increment for {poolName}, flag was not set.");
                }
            }

            // Decrease spawn rate while maintaining a minimum limit
            spawnRate = Mathf.Max(minimumSpawnRate, spawnRate - spawnRateReduction);
        }
    }

    private IEnumerator SpawnWave()
    {
        // Copy the pool names into a list to shuffle
        List<string> shuffledPoolNames = new List<string>(enemyPoolNames);

        // Fisher-Yates shuffle implementation
        for (int i = 0; i < shuffledPoolNames.Count; i++)
        {
            // Pick a random index starting from the current index
            int randomIndex = Random.Range(i, shuffledPoolNames.Count);

            // Swap the elements
            string temp = shuffledPoolNames[i];
            shuffledPoolNames[i] = shuffledPoolNames[randomIndex];
            shuffledPoolNames[randomIndex] = temp;
        }

        // Debugging: Check the shuffled order
        Debug.Log("Shuffled Order: " + string.Join(", ", shuffledPoolNames));

        // Iterate through the shuffled pool names to spawn enemies in random order
        foreach (string poolName in shuffledPoolNames)
        {
            int count = enemyCountPerType[poolName];
            Debug.Log($"Spawning {count} of {poolName}.");

            for (int i = 0; i < count; i++)
            {
                // Randomize spawn position around the player
                Vector3 spawnDirection = Random.insideUnitSphere * spawnDistance;
                spawnDirection += playerTransform.position;
                spawnDirection.z = 0;  // Keep the enemy on the same 2D plane

                // Fetch from the object pool using the pool name
                GameObject enemy = ObjectPool.Instance.GetPooledObject(poolName);
                if (enemy != null)
                {
                    enemy.transform.position = spawnDirection;
                    enemy.transform.rotation = Quaternion.identity;
                }
                else
                {
                    Debug.LogWarning($"No pooled object available for {poolName}.");
                }
            }

            yield return new WaitForSeconds(spawnRate);
        }

        isSpawning = false;
    }
}