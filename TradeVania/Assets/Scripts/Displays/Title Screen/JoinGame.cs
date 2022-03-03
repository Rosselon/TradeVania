using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinGame : MonoBehaviour
{
    [SerializeField] private GameObject hostJoinPage = null;
    [SerializeField] private TMP_InputField IPInput = null; 
    [SerializeField] private Button joinGameButton = null;

    // Sub and unsub to the connect and disconnect events
    private void OnEnable()
    {
        MyNetworkManager.ClientConnects += ClientConnects;
        MyNetworkManager.ClientRejected += ClientRejected;
    }
    private void OnDisable()
    {
        MyNetworkManager.ClientConnects -= ClientConnects;
        MyNetworkManager.ClientRejected -= ClientRejected;
    }
    
    public void JoinClick()
    {
        // Get the IP address the user input
        string IP = IPInput.text;

        // Set the target network to be the IP address
        NetworkManager.singleton.networkAddress = IP;

        // Start the client
        NetworkManager.singleton.StartClient();

        // Stop join game button being clicked more than once
        joinGameButton.interactable = false;
    }

    private void ClientConnects()
    {
        // Reactivate Join Game Button (if the player leaves the lobby)
        joinGameButton.interactable = true;

        // Load the lobby page and hide all other pages if manage to connect
        gameObject.SetActive(false);
        hostJoinPage.SetActive(false);
    }
    private void ClientRejected()
    {
        // Reactivate Join Game Button
        joinGameButton.interactable = true;
    }
}
