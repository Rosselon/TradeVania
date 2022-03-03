using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Set health bar in inspector
    [SerializeField] private Image healthBar;

    // Initialise components
    private Damage damage;
    

    private void Awake()
    {
        // Set variables
        damage = GetComponent<Damage>();

        // Subscribe to the HealthChangeClient event
        damage.HealthChangeClient += UpdateHealthBar;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the HealthChangeClient event
        damage.HealthChangeClient -= UpdateHealthBar;
    }

    // Update the fill of the image
    private void UpdateHealthBar(int currentHealth, int fullHealth)
    {

        healthBar.fillAmount = (float) currentHealth / fullHealth;
    }

}
