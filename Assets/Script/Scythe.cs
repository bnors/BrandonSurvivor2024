using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : MonoBehaviour, IPoolable
{
    public float lifetime = 2f;
    public int damage = 25;  // Damage value that Scythe deals

    public void Reset()
    {
        lifetime = 2;  // Reset the lifetime
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            gameObject.SetActive(false);  // Deactivate the scythe instead of destroying it
        }

        transform.position += transform.right * 5f * Time.deltaTime;  // Movement of the scythe
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))  // Check if it collides with an enemy
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);  // Apply damage to the enemy
            }
        }
    }
}
