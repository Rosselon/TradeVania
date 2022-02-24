using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CameraManager : NetworkBehaviour
{
    // Get the camera transform
    [SerializeField] private Transform camTransform = null;

    // Only runs on the client that has authority over this object
    public override void OnStartAuthority()
    {
        // Activates the camera only for the client that owns the player the camera is attached to
        camTransform.gameObject.SetActive(true);
        
        // Call the server to move the cameras to where they need to be
        CmdMoveCamera();        
    }

    [Command]
    public void CmdMoveCamera()
    {
        // Return if host
        if(connectionToClient.connectionId == 0) {return;}
            
        // Turn Camera around
        gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
    }     
}
