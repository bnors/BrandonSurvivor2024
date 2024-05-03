using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingWeapon : MonoBehaviour
{
    public Transform player;                // Reference to the player's transform
    public float orbitDistance = 1.5f;      // Distance from the player
    public float orbitSpeed = 180.0f;       // Speed of orbiting around the player

    void Update()
    {
        if (player != null)
        {
            OrbitAroundPlayer();
        }
    }

    void OrbitAroundPlayer()
    {
        transform.position = player.position + (transform.position - player.position).normalized * orbitDistance;
        transform.RotateAround(player.position, Vector3.forward, orbitSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            int damage = 25;                                // Set the damage value
            other.GetComponent<Enemy>().TakeDamage(damage); // Apply damage
            SoundPlayer.GetInstance().PlayOrbitHitAudio();  // Play the orbit weapon hit sound
        }
    }
}
