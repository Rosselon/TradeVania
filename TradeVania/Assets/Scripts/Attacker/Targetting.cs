using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Targetting : NetworkBehaviour
{
    // Set the target size sprite
    [SerializeField] private GameObject targetSprite = null;

    // Set the distance an attacker can target another object from
    [SerializeField] private float targetRange = 5f;

    // Game object being targeted
    [SerializeField] private GameObject target = null;

    #region Server
    public override void OnStartServer()
    {
        // Set the collider to be the attackers target range
        this.gameObject.GetComponent<SphereCollider>().radius = targetRange / 2;

        // Set the size of the visualisation of the targetRange
        targetSprite.transform.localScale = new Vector3(targetRange, targetRange, 1);

        base.OnStartServer();
    }
    
    [ServerCallback]
    private void OnTriggerEnter(Collider hit)
    {
        // Don't interact with the floor
        if (hit.gameObject.name == "Ground"){return;}

        // Don't interact with objects also owned by you
        if(hit.gameObject.GetComponent<NetworkIdentity>().connectionToClient.connectionId == 
            gameObject.GetComponent<NetworkIdentity>().connectionToClient.connectionId) {return;}

        // Set the target to be the object you collided with
        target = hit.gameObject;

        // Disable the movement script
        gameObject.GetComponent<Movement>().enabled = false;
    }
    #endregion

    #region Client
    
    // Runs on all clients when this object is created
    public override void OnStartClient()
    {
        // Update the range UI
        targetSprite.transform.localScale = new Vector3(targetRange, targetRange, 1);
        
        base.OnStartClient();
    }
    #endregion
}
