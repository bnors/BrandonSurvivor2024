using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MermanWeakPoint : MonoBehaviour
{
    [SerializeField] private Enemy mermanEnemy; // Reference to the main Enemy script
    [SerializeField] private int damageMultiplier = 2; // Damage multiplier

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Scythe") || collision.CompareTag("Hammer"))
        {
            int weaponDamage = 0;

            Scythe scytheWeapon = collision.GetComponent<Scythe>();
            if (scytheWeapon != null)
            {
                weaponDamage = scytheWeapon.GetDamage();
            }

            OrbitingWeapon orbitingWeapon = collision.GetComponent<OrbitingWeapon>();
            if (orbitingWeapon != null)
            {
                weaponDamage = orbitingWeapon.GetDamage();
            }

            int increasedDamage = weaponDamage * damageMultiplier;
            mermanEnemy.TakeDamage(increasedDamage);

            // Play the weak point hit audio
            SoundPlayer.GetInstance().PlayWeakPointHitAudio();

            Debug.Log("Merman's weak point hit with extra damage!");
        }
    }
}
