using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Mirror;
using System;

public class GameOverScreen : MonoBehaviour
{
    // Set up ui to display the winner
    [SerializeField] TMP_Text winnerText;
    [SerializeField] GameObject gameOverScreen;
    private void Start()
    {
        // Sub to game over event
        Death.GameOverEvent += GameOverServer;
    }

    private void OnDestroy()
    {
        // Unsub from game over event
        Death.GameOverEvent -= GameOverServer;
    }

    private void GameOverServer(string winner)
    {
        // Set the UI to show the name of the winner
        winnerText.text = winner;

        // Activate the game over screen 
        gameOverScreen.SetActive(true);
    }

    // Button to leave the game once the game has finished
    public void LeaveButton()
    {
        // Leave for both host or client
        if(NetworkServer.active && NetworkClient.isConnected)
        {
            // Stop hosting
            NetworkManager.singleton.StopHost();
        }
        else 
        {
            // Leave as a client
            NetworkManager.singleton.StopClient();
        }
    }
}
