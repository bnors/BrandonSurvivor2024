using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }  // Singleton instance

    [SerializeField] private TextMeshProUGUI scytheDamageText;  // Reference to Scythe damage text

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

    // Method to update Scythe damage text
    public void UpdateScytheDamageText(int damage)
    {
        if (scytheDamageText != null)
        {
            scytheDamageText.text = $"Scythe Damage: {damage}";
        }
    }
}
