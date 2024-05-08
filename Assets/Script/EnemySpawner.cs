using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [SerializeField] string[] enemyPoolNames;
    [SerializeField] float initialSpawnRate = 1f;
    [SerializeField] float spawnRateReduction = 0.1f;
    [SerializeField] int initialWaveSize = 1;
    [SerializeField] int incrementPerWave = 1;
    [SerializeField] float spawnDistance = 10f;
    [SerializeField] float minimumSpawnRate = 0.5f;
    [SerializeField] float waveInterval = 10f;

    private Transform playerTransform;
    private float spawnRate;
    private Dictionary<string, int> enemyCountPerType;
    private Dictionary<string, bool> encounterFlagPerType;
    private bool spawningEnabled = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        ResetSpawnSettings();
        StartCoroutine(SpawnWaves());
    }

    // Reset enemy counts and encounter flags
    private void ResetSpawnSettings()
    {
        spawnRate = initialSpawnRate;
        enemyCountPerType = new Dictionary<string, int>();
        encounterFlagPerType = new Dictionary<string, bool>();

        foreach (string poolName in enemyPoolNames)
        {
            enemyCountPerType[poolName] = initialWaveSize;
            encounterFlagPerType[poolName] = false;
        }
    }

    public void RegisterEnemyEncounter(string poolName)
    {
        if (encounterFlagPerType.ContainsKey(poolName))
        {
            encounterFlagPerType[poolName] = true;
            Debug.Log($"Encounter flag set for {poolName}.");
        }
        else
        {
            Debug.LogWarning($"Pool name {poolName} not found in encounter flags.");
        }
    }

    public void StopSpawning()
    {
        spawningEnabled = false;
    }

    public void StartSpawning()
    {
        spawningEnabled = true;
        ResetSpawnSettings();
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (spawningEnabled)
        {
            yield return new WaitForSeconds(waveInterval);

            Debug.Log("Wave completed. Checking encounter flags...");
            foreach (string poolName in enemyPoolNames)
            {
                if (encounterFlagPerType[poolName])
                {
                    enemyCountPerType[poolName] += incrementPerWave;
                    encounterFlagPerType[poolName] = false;
                    Debug.Log($"Incremented count for {poolName} to {enemyCountPerType[poolName]}");
                }
                else
                {
                    Debug.Log($"No increment for {poolName}, flag was not set.");
                }
            }

            spawnRate = Mathf.Max(minimumSpawnRate, spawnRate - spawnRateReduction);

            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        // Choose a single enemy type randomly for this wave
        string poolName = enemyPoolNames[Random.Range(0, enemyPoolNames.Length)];
        int count = enemyCountPerType[poolName];
        Debug.Log($"Spawning {count} of {poolName}.");

        for (int i = 0; i < count; i++)
        {
            // Randomize spawn position around the player
            Vector3 spawnDirection = Random.insideUnitSphere * spawnDistance;
            spawnDirection += playerTransform.position;
            spawnDirection.z = 0;  // Ensure the enemy remains on the 2D plane

            // Fetch from the object pool using the chosen pool name
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

        // Wait for the specified spawn rate before ending this wave
        yield return new WaitForSeconds(spawnRate);
    }
}