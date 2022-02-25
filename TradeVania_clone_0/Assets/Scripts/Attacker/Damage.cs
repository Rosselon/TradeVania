using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Damage : NetworkBehaviour
{
    // Adjustable attributes
    [SerializeField] private int fullHealth = 50;


    // Using events so that other scripts can listen in without being tied to this script
    public event Action OnDeathServer;

    // Event called on the client when health is changed (for ui etc)
    public event Action<int, int> HealthChangeClient;

    

    // Using a synchvar for current health so that all clients will have this value updated
    [SyncVar (hook = nameof(HealthChanged))] private int health;

    #region Server

    // Only Deal Damage on the server
    public override void OnStartServer()
    {
        // Initialise the health to be full when this scipt bearing object is created
        health = fullHealth;
    }

    // A function to damage this script bearing object
    [Server] public void DoDamage(int damage)
    {
        // If the health is already 0 then don't bother damaging it
        if(health == 0){return;}

        // Reduce the health by the damage but don't go below 0 
        health = Mathf.Max(health - damage, 0);

        if (health == 0)
        {
            // Invoke on death server so any other script can activate corresponding functions
            OnDeathServer?.Invoke();

            Debug.Log("We Died");
        } 
    }

    #endregion

    #region Client

    // Invoke the health change event whenever health syncvar changes
    private void HealthChanged(int oldHealth, int newHealth)
    {
        // Pass in the new health and the max health
        HealthChangeClient?.Invoke(newHealth, fullHealth);
    }

    #endregion
}
