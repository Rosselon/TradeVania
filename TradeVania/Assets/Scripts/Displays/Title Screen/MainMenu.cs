using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject HostJoinPage = null;

    public void HostClick()
    {
        // Disappear the host page
        HostJoinPage.SetActive(false);

        // Mirror function to start hosting
        NetworkManager.singleton.StartHost();
    }

}
