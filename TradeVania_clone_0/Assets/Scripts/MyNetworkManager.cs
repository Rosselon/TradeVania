using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MyNetworkManager : NetworkManager
{

    // Set up the base prefab
    [SerializeField] private GameObject basePrefab = null;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        
        // Set the orientation of the base so that it is facing the opposition
        Quaternion orientation = conn.connectionId == 0 ? conn.identity.transform.rotation : Quaternion.Euler(0, 180, 0);

        // Create the base on the server when the player is added
        GameObject baseInstance = Instantiate(basePrefab,
                                            conn.identity.transform.position,   // Use the location and roatation of the player
                                            orientation);
        
        // Spawn the GameObject for all clients 
        NetworkServer.Spawn(baseInstance,       
                            conn);          // Assign ownership to the player that was spawned

        
    }
}
