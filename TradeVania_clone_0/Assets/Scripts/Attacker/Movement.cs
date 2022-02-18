using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    // The server controls the movement of the unit automatically
    [Server] private void MoveUnit()
    {
        // Move the player forward on its z axis (as the attackers are rotated appropriately already)
        transform.Translate(new Vector3(0, 0, moveSpeed * Time.deltaTime));         // Moderate the movement speed using Time.deltaTime so its not affected by framerate
    }

    // Called once per frame on the server
    [ServerCallback] private void Update()
    {
        MoveUnit();
    }
}
