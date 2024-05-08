using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }  // Singleton instance

    [SerializeField] private TextMeshProUGUI scytheDamageText;
    [SerializeField] private TextMeshProUGUI orbitingWeaponDamageText;  // New field

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Make persistent across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateScytheDamageText(int damage)
    {
        if (scytheDamageText != null)
        {
            scytheDamageText.text = $"Scythe Damage: {damage}";
        }
    }

    public void UpdateOrbitingWeaponDamageText(int damage)
    {
        if (orbitingWeaponDamageText != null)
        {
            orbitingWeaponDamageText.text = $"Hammer Damage: {damage}";
        }
    }
}