using UnityEngine;
using System.IO;

// Used for saving to binary format
using System.Runtime.Serialization.Formatters.Binary;

// Static as does not need to be instantiated to run    
public static class SaveLoad
{
    // Set the path to save the stats file
    private static string PATH = Application.persistentDataPath + "/Stats.data";

    // Save data from a stats system
    public static void SaveStats(Stats stats)
    {
        // Create a binary formatter to convert our data into unreadable binary code
        BinaryFormatter bf = new BinaryFormatter();

        // Create a file stream to create a file at the path location
        FileStream fs = new FileStream(PATH, FileMode.Create);

        // Create an instance of the playerstats file
        PlayerStats ps = new PlayerStats(stats);

        // Write data to the file
        bf.Serialize(fs, ps);
        fs.Close();
    }
    public static PlayerStats LoadStats()
    {
        // If there is a no file at the given path then return
        if (!File.Exists(PATH)){return null;}
        
        // Create a binary formatter to convert our data back to readable code
        BinaryFormatter bf = new BinaryFormatter();

        // Open the existing file at the point path
        FileStream fs = new FileStream(PATH, FileMode.Open);
            
        // Get the player data object from binary format
        PlayerStats ps = bf.Deserialize(fs) as PlayerStats;
        fs.Close();
            
        return ps;
    }
}


