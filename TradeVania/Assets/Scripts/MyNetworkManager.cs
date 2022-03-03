using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SceneManagement;

public class MyNetworkManager : NetworkManager
{

    // Scene names
    private string PLAYSCENE = "MapScene";

    // Set up the base prefab
    [SerializeField] private GameObject basePrefab = null;

    // Variable to see if game is in progress
    private bool gameStarted = false;

    // List of the two players
    public List<MyPlayer> Players = new List<MyPlayer>(); 

    // Client connections actions
    public static event Action ClientConnects;
    public static event Action ClientRejected;

    #region Server

    // Run on the server when a client connects 
    public override void OnServerConnect(NetworkConnection conn)
    {
        if(gameStarted)
        {
            // If the game has started disconnect any players trying to connect
            conn.Disconnect();
        }
    }

    // Start the game
    public void StartGame()
    {
        Debug.Log($"Enough players to start game: {Players.Count == 2}");
        //if(MyNetworkManager.singleton.numPlayers == 2)
        if(Players.Count == 2)
        {
            // Say the game has started
            gameStarted = true;

            // Change to the starting scene
            ServerChangeScene(PLAYSCENE);
        }
    }

    public override void OnServerSceneChanged(string newSceneName)
    {
        Debug.Log($"active scene: {newSceneName}, play scene: {PLAYSCENE}");
        // If we change do the map scene do this
        if(newSceneName == PLAYSCENE)
        {
            foreach(MyPlayer player in Players)
            {
                Transform startpos = GetStartPosition();
                Debug.Log($"Rotation {startpos.rotation}, Position {startpos.position}");
                // Instantiate each player
                // Set the orientation of the base so that it is facing the opposition
                Quaternion orientation = player.connectionToClient.connectionId == 0 ? startpos.rotation : Quaternion.Euler(0, 180, 0);

                // Create the base on the server when the player is added
                GameObject baseInstance = Instantiate(basePrefab,
                                                    startpos.position,   // Use the location and roatation of the player
                                                    orientation);
                
                // Spawn the GameObject for all clients 
                NetworkServer.Spawn(baseInstance,       
                                    player.connectionToClient);          // Assign ownership to the player that was spawned
            }           
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
         
        // Set the player to be this connections player
        MyPlayer player = conn.identity.GetComponent<MyPlayer>();

        // Add player to list of players
        Players.Add(player); 

        // Set the players party ownership status if only one player is connected they are the owner
        //if(MyNetworkManager.singleton.numPlayers == 1) player.SetOwnParty(true);
        Debug.Log($"Player {conn.connectionId} party owner : {Players.Count == 1}");
        Debug.Log($"Num Players = {MyNetworkManager.singleton.numPlayers}, Players list size = {Players.Count}");
        player.SetOwnParty(Players.Count == 1);
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        // Remove player from the list when they disconnect
        Players.Remove(conn.identity.GetComponent<MyPlayer>());

        base.OnServerDisconnect(conn);
    }
    public override void OnStopServer()
    {
        // Clear the player list
        Players.Clear();

        // Set the game to be not started
        gameStarted = false;
    }
    #endregion

    #region Client

    // When Client Connects or disconnects do normal stuff but also invoke corresponding actions
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        
        // Invoke Client connects event
        ClientConnects?.Invoke();
    }
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        // Invoke client rejected event
        ClientRejected?.Invoke();
    }

    public override void OnStopClient()
    {
        Players.Clear();
    }
    
    #endregion
    
}
