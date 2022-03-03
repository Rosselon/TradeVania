using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePendingPage : MonoBehaviour
{
    [SerializeField] private GameObject pendingPage = null;
    [SerializeField] private Button startButton = null;
    [SerializeField] private TMP_Text[] textArray = new TMP_Text[2];

    private void Start()
    {
        MyNetworkManager.ClientConnects += ClientConnects;
        MyPlayer.PartyOwnershipUpdated += OwnershipUpdated;
        MyPlayer.PlayerNameUpdated += NameUpdated;
    }
    private void OnDestroy()
    {
        MyNetworkManager.ClientConnects -= ClientConnects;
        MyPlayer.PartyOwnershipUpdated -= OwnershipUpdated;
        MyPlayer.PlayerNameUpdated -= NameUpdated;
    }

    private void ClientConnects()
    {
        // Load the game pending page
        pendingPage.SetActive(true);
    }

    public void BackClick()
    {
        // If you are a server and a client (are host)
        if(NetworkServer.active && NetworkClient.isConnected)
        {
            // Stop hosting if are a host
            NetworkManager.singleton.StopHost();
        }
        else 
        {
            // Stop being a client if not a host
            NetworkManager.singleton.StopClient();

            // Reload the main menu scene
            SceneManager.LoadScene(0);
        }
    }

    private void OwnershipUpdated(bool owns)
    {
        // Set the button to be active if you own it and not active if you don't
        startButton.gameObject.SetActive(owns);
    }

    public void StartGame()
    {
        // Call the command in this player
        NetworkClient.connection.identity.GetComponent<MyPlayer>().CmdStartGame();
    }

    private void NameUpdated()
    {   
        // Get the players list from the networkmanager
        List<MyPlayer> players = ((MyNetworkManager)NetworkManager.singleton).Players;
        
        if (players.Count < 1) {return;}

        // Update the text for the host
        textArray[0].text = players[0].GetPlayerName();

        // Update the text for the client if they exist
        textArray[1].text = players.Count == 2 ? players[1].GetPlayerName() : "player pending...";

        // Only allow the host to start the game if there are two players 
        startButton.interactable = players.Count == 2;
    }

}
