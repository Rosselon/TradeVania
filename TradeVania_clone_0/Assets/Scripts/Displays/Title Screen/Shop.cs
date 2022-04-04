using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{

    private Stats stats;

    [SerializeField] private TMP_Text bankText;

    private void Start()
    {
        stats = GameObject.FindObjectOfType<Stats>();
    }

    public void UpdateBank()
    {
        // Set the coin text to be the stored amount of coins
        bankText.text = stats.TVCoins.ToString();
    }
}
