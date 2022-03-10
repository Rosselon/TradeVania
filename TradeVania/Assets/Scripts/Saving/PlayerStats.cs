using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This allows us to save this class to a file
[System.Serializable]
public class PlayerStats
{
    // Variable for in game currency
    public int TVCoins;

    // Variables for number of each troop type owned
    public int numBasic;
    public int numSniper;
    public int numBrute;

    public int[] numTroops = new int[3]{0 , 0, 0};
    // Constructor for this class
    public PlayerStats(Stats stats)
    {
        // Create player stats
        TVCoins = stats.TVCoins;
        numBasic = stats.numBasic;
        numSniper = stats.numSniper;
        numBrute = stats.numBrute;

        numTroops[0] = numBasic;
        numTroops[1] = numSniper;
        numTroops[2] = numBrute;
    }
    
}
