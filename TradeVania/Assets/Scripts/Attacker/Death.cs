using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Death : NetworkBehaviour
{
    private Damage damage;

    [SerializeField] private GameOverScreen gameOverScreen;

    public static event Action<string> GameOverEvent;

    // Run on server when this object is created
    public override void OnStartServer()
    {
        // Set the damage component to be the one on this object
        damage = GetComponent<Damage>();

        // Sub to damages on death event
        damage.OnDeathServer += DeathServer;
    }
    // Run on server when this object is destroyed
    public override void OnStopServer()
    {
        // Unsub from damages on death event
        damage.OnDeathServer -= DeathServer;
    }

    // Destroy self when die
    private void DeathServer()
    {
        // If the death occurs to a spawner then one player has won
        if(TryGetComponent<Spawner>(out Spawner spawner))
        {
           // Set the winner text  
           string winner = spawner.GetComponent<NetworkIdentity>().connectionToClient.connectionId == 0 
                        ? "Player 1 Wins" : "Player 2 Wins"; 

            // End the game
            GameOverEvent?.Invoke(winner);
            RpcDeathOnClient(winner);

        }        
        NetworkServer.Destroy(gameObject);
    }
    [ClientRpc] private void RpcDeathOnClient(string winner)
    {
        // End the game on the client
        GameOverEvent?.Invoke(winner);
    }
}
