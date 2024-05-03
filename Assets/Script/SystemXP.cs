using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemXP : MonoBehaviour
{
    public int xpValue = 10; // Amount of XP this shard gives

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("XP shard picked up by player.");
            other.GetComponent<Player>().AddXP(xpValue);
            Destroy(gameObject);
        }
    }
}
