using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spawner : NetworkBehaviour
{

    // Set up a layer mask for what you can spawn on
    [SerializeField] private LayerMask layerMask = new LayerMask();

    // Get the prefab for the unit you want to spawn
    [SerializeField] private GameObject AttackerPrefab = null;

    // Set the distance from the main base that you can spawn a player
    [SerializeField] private float spawnDistance = 7.5f;

    private Quaternion spawnRotation = Quaternion.identity;

    // Initialise main camera (for ray casting)
    private Camera mainCamera;

    // Region for the code called on the server
    #region Server

    // Command on the server to spawn an attacker
    [Command] private void CmdSpawnAttacker(Vector3 Spawnpos)
    {
        // Set the objects to initially face the correct direction
        if (this.connectionToClient.connectionId != 0){spawnRotation = Quaternion.Euler(0, 180, 0);}     //As there are only 2 players you only need to check if the client isn't player one to decide the orientation of its attackers

        // Create the Attacker on the Server
        GameObject AttackerInstance = Instantiate(AttackerPrefab,           // The prefab for the attacker trying to be spawned in
                                                  Spawnpos,   // Get the position of the spawn location transform
                                                  spawnRotation);  // Get the rotation

        // Spawn the GameObject for all clients (make sure is in instantiatable prefabs)
        NetworkServer.Spawn(AttackerInstance,       // Use the above spawned attacker
                            connectionToClient      // Assign this to the owner of the game object this script is attached to
                            );
    
    }

    #endregion

    // Region of the the code called on the client
    #region Client

    // Run on the client side whent this object is created
    public override void OnStartAuthority()
    {
        // Set the main camera
        mainCamera = Camera.main;
    }

    [ClientCallback]       //Prevents the server from trying to run this method (as update runs on both client and server)
    private void Update()
    {
        // Return if haven't clicked
        if(!Mouse.current.leftButton.wasPressedThisFrame) {return;}  // Return if you aren't clicking the right mouse button
        
        // Only attempt to spawn if the player has authority
        if(!hasAuthority){return;}

        // Figure out where we clicked
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());     // Creates a ray from camera to mouse position

        //Checks ray hits smth and gets the hit point
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)){return;}     // The raycast hit has a point variable which is a point in the world that the ray hit

        // Only Spawn Units within a certain range of the players base
        if(hit.point.z < this.transform.position.z - spawnDistance || 
            hit.point.z > this.transform.position.z + spawnDistance) 
            {return;}

        // Run the the server spawn function
        CmdSpawnAttacker(hit.point);   
    }

    #endregion
}
