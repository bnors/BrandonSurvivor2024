using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemXP : MonoBehaviour
{
    public int xpValue = 10;
    public float detectionRadius = 5f; // Detection range within which the XP shard moves toward the player
    public float movementSpeed = 3f;   // Speed at which the XP shard moves toward the player
    private Transform playerTransform;
    private bool isAttracted = false;

    private void Start()
    {
        // Find the player's transform by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // Check if the player is within the detection radius
            if (distanceToPlayer <= detectionRadius)
            {
                isAttracted = true;
            }

            // Move the XP shard towards the player if it is attracted
            if (isAttracted)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                transform.position += direction * movementSpeed * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the player collects the shard, grant XP and destroy the shard
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().AddXP(xpValue);

            SoundPlayer.GetInstance().PlayXPCollectAudio();

            Destroy(gameObject);
        }
    }
}
