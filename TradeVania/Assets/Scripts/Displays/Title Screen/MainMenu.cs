using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void HostClick()
    {
        // Mirror function to start hosting
        NetworkManager.singleton.StartHost();
    }
}
