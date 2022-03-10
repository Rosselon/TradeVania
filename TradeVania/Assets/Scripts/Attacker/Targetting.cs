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

    // Returns the current target to any script that requests it
    public GameObject GetTarget()
    {
        return target;
    }
    public float GetTargetRange()
    {
        return targetRange;
    }

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
        // All conditions to leave this function
        #region Leave Conditions
        // Don't interact if already have a target
        if(target != null){return;}

        // Don't interact with the floor
        if (hit.gameObject.layer != LayerMask.NameToLayer("Targettable")){return;}
        
        // Only interact with cube colliders
        if (hit.GetType() != typeof(BoxCollider)){return;}

        // Don't interact with objects also owned by you
        if(hit.gameObject.GetComponent<NetworkIdentity>().connectionToClient.connectionId == 
            gameObject.GetComponent<NetworkIdentity>().connectionToClient.connectionId) {return;}

        #endregion

        // Set the target to be the object you collided with
        target = hit.gameObject;

        // Change movement to follow target
        gameObject.GetComponent<Movement>().SetMoveForward(false);
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
