using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage = 8;
    public float speed = 5f;
    private Vector2 direction;

    // Set the direction of the fireball towards the target
    public void Initialize(Vector2 targetPosition)
    {
        // Calculate the direction from the fireball's position to the target position
        direction = (targetPosition - (Vector2)transform.position).normalized;
    }

    // Set the damage of the fireball
    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }

    private void Update()
    {
        // Move in the specified direction at the defined speed
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if it hits the player
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject); // Destroy fireball on impact
        }
    }
}