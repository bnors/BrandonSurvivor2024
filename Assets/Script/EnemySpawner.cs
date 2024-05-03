using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    public IEnumerator SpawnEnemy()
    {
        while (true)
        {

            for (int i = 0; i < 10; i++)
            {
                Instantiate(enemyPrefab, new Vector3(3f, 3f, 0), Quaternion.identity);
            }

            yield return new WaitForSeconds(5);
        
        }
    }

}
