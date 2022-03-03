using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyPlayer : NetworkBehaviour
{
    // Track if this player is the party owner (to decide if they have access to the start game button)
    [SyncVar(hook = nameof(PartyOwnershipUpdate))]
    private bool ownParty = false;

    // Ownership updated event
    public static event Action<bool> PartyOwnershipUpdated;

    // Party ownership getter
    public bool GetOwnParty()
    {
        return ownParty;
    }

    // Set Party onwership
    [Server] public void SetOwnParty(bool ownership)
    {
        ownParty = ownership;
    }


    [Command] public void CmdStartGame()
    {
        // Try to start the game if the player is the party owner
        if(ownParty)
        {
            Debug.Log("CmdStarted game");
            ((MyNetworkManager)NetworkManager.singleton).StartGame();
        }
    }

    public override void OnStartClient()
    {
        if(NetworkServer.active) {return;}

        ((MyNetworkManager)NetworkManager.singleton).Players.Add(this);
    }

    public override void OnStopClient()
    {
        if(!isClientOnly) {return;}

        ((MyNetworkManager)NetworkManager.singleton).Players.Remove(this);
    }
    
    private void PartyOwnershipUpdate(bool old, bool current)
    {
        if(hasAuthority)
        {
            PartyOwnershipUpdated?.Invoke(current);
        }
    }
}
