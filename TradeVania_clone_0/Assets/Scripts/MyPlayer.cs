using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyPlayer : NetworkBehaviour
{
    // Track if this player is the party owner (to decide if they have access to the start game button)
    [SyncVar(hook = nameof(PartyOwnershipUpdate))] private bool ownParty = false;

    // Sync the name of the player
    [SyncVar(hook = nameof(PlayerNameUpdate))] private string playerName = "";

    
    // Ownership updated event
    public static event Action<bool> PartyOwnershipUpdated;
    public static event Action PlayerNameUpdated;

    #region Server Code

        #region Getters    
    // Party ownership getter
    public bool GetOwnParty()
    {
        return ownParty;
    }

    // Player Name Getter
    public string GetPlayerName()
    {
        return playerName;
    }

        #endregion

        #region  Setters
    // Set Party onwership
    [Server] public void SetOwnParty(bool ownership)
    {
        ownParty = ownership;
    }
    [Server] public void SetPlayerName(string name)
    {
        playerName = name;
    }
        #endregion

    [Command] public void CmdStartGame()
    {
        // Try to start the game if the player is the party owner
        if(ownParty)
        {
            Debug.Log("CmdStarted game");
            ((MyNetworkManager)NetworkManager.singleton).StartGame();
        }
    }
    public override void OnStartServer()
    {
        // Prevents the player from being deleted when loading new screen (for server)
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Client Code

    public override void OnStartClient()
    {
        if(NetworkServer.active) {return;}

        // Prevents the player from being deleted when loading new screen (for client)
        DontDestroyOnLoad(gameObject);

        ((MyNetworkManager)NetworkManager.singleton).Players.Add(this);
    }

    public override void OnStopClient()
    {
        // Update when the player disconnects
        PlayerNameUpdated?.Invoke();

        if(!isClientOnly) {return;}

        ((MyNetworkManager)NetworkManager.singleton).Players.Remove(this);
    }
    
        #region Syncvar functions
    // Code run when the party ownership syncvar is updated
    private void PartyOwnershipUpdate(bool old, bool current)
    {
        if(hasAuthority)
        {
            PartyOwnershipUpdated?.Invoke(current);
        }
    }

    private void PlayerNameUpdate(string oldName, string newName)
    {
        // Invoke the player name updated function
        PlayerNameUpdated?.Invoke();
    }

        #endregion


    #endregion
}
