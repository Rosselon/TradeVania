using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{

    private Stats stats;

    [SerializeField] private TMP_Text bankText;
    [SerializeField] private TMP_Text basicText;
    [SerializeField] private TMP_Text sniperText;
    [SerializeField] private TMP_Text bruteText;


    private void Start()
    {
        stats = GameObject.FindObjectOfType<Stats>();
    }

    #region Update Functions
    public void UpdateBank()
    {
        // Set the coin text to be the stored amount of coins
        bankText.text = stats.TVCoins.ToString();
    }

    public void UpdateBasic()
    {
        // Set the coin text to be the stored amount of coins
        basicText.text = stats.numBasic.ToString();
    }

    public void UpdateSniper()
    {
        // Set the coin text to be the stored amount of coins
        sniperText.text = stats.numSniper.ToString();
    }
    public void UpdateBrute()
    {
        // Set the coin text to be the stored amount of coins
        bruteText.text = stats.numBrute.ToString();
    }

    public void UpdateAllShop()
    {
        UpdateBank();
        UpdateBasic();
        UpdateSniper();
        UpdateBrute();

    }
    #endregion
}
