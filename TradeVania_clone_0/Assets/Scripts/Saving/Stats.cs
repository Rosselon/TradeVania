using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // Variable for in game currency
    [SerializeField] public int TVCoins;

    // Variables for number of each troop type owned
    [SerializeField] public int numBasic;
    [SerializeField] public int numSniper;
    [SerializeField] public int numBrute;
    public PlayerStats ps;

    public static Stats stats;

    private Shop shop;
 
    void Awake()
    {
        // Singleton system (so there is only ever one stats system)
        if(stats == null)
        {
           stats = this;
           DontDestroyOnLoad(gameObject);
        }
        else if(stats != this)
        {
           Destroy(gameObject);
        }
        // Subscribe to gameover event to add money
        Death.GameOverEvent += GameOverAddMoney;
    }

    void OnDestroy()
    {
        Death.GameOverEvent -= GameOverAddMoney;
    }

    private void GameOverAddMoney(string name)
    {
        TVCoins += 100;
    }

    private void Start()
    {        
        // Set the shop variable
        shop = GameObject.FindObjectOfType<Shop>();
        
        // Initial load
        Load();

        // Update the banks
        shop.UpdateBank();
    }

    public void Save()
    {
        SaveLoad.SaveStats(this);
    }
    public void Load()
    {
        // Get the player stats from the binary save file
        ps = SaveLoad.LoadStats();

        if(ps == null){return;}

        // Apply the variables
        TVCoins = ps.TVCoins;
        numBasic = ps.numBasic;
        numSniper = ps.numSniper;
        numBrute = ps.numBrute;
    }

    public void BuyTroop(int id)
    {
        switch(id) 
        {
            case 0:
                if (TVCoins < BasicTroop.cost){return;}
                TVCoins -= BasicTroop.cost;
                numBasic += 1;
                break;
            case 1:
                if (TVCoins < SniperTroop.cost){return;}
                TVCoins -= SniperTroop.cost;
                numSniper += 1;
                break;
            case 2:
                if (TVCoins < BruteTroop.cost){return;}
                TVCoins -= BruteTroop.cost;
                numBrute += 1;
                break;
            default:
                break;
        }
        // Update the bank details
        shop.UpdateBank();
        Save();
        Load();
    }
}
