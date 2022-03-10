using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private TMP_Text bankText;

    public void UpdateBank(int newBalance)
    {
        // Set the coin text to be the stored amount of coins
        bankText.text = newBalance.ToString();
    }
}
