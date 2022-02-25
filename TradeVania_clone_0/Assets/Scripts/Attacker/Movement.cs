using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
public class Movement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    // The agent for this object
    private NavMeshAgent navAgent = null;

    private Targetting targetting;

    // Variables
    private bool moveForward = true;
    private Quaternion startingRotation;

    public void SetMoveForward(bool option)
    {
        moveForward = option;
    }

    #region Server

    public override void OnStartServer()
    {
        // Get this objects targetting script
        targetting = this.GetComponent<Targetting>();

        // Assign the navAgent to be the one attached to this script bearing object
        navAgent = this.GetComponent<NavMeshAgent>();
        navAgent.speed = moveSpeed;
        navAgent.stoppingDistance = targetting.GetTargetRange() - 0.2f;

        // Get the starting rotation of the object
        startingRotation = this.gameObject.transform.rotation;
    }

    // Called once per frame on the server
    [ServerCallback] private void Update()
    {
        // If the move foward variable is true continually move forward
        if(moveForward) MoveUnitForward();

        // Otherwise track your current target
        else FollowTarget();
    }
    
    // The server controls the movement of the unit automatically
    [Server] private void MoveUnitForward()
    {
        // Make sure the attacker is facing forward
        this.gameObject.transform.rotation = startingRotation;

        // Move the player forward on its z axis (as the attackers are rotated appropriately already)
        transform.Translate(new Vector3(0, 0, moveSpeed * Time.deltaTime));         // Moderate the movement speed using Time.deltaTime so its not affected by framerate
    }
    [Server] private void FollowTarget()
    {
        // Get the players target
        GameObject target = GetComponent<Targetting>().GetTarget();
        
        // If there is no target start moving forward again
        if (target == null) 
        {
            moveForward = true;

            // Reset the navMesh path
            navAgent.ResetPath();

            return;
        }
        
        // Stop attacker from moving if withing the stopping distance range
        navAgent.updatePosition = navAgent.remainingDistance > navAgent.stoppingDistance;
        

        // If target position within the navmesh
        if(NavMesh.SamplePosition(target.transform.position, 
                                out NavMeshHit point,   // Sets the point variable to be the transform of the desired location
                                1f,                     // Sets how far from target position to set the target
                                NavMesh.AllAreas)) 
        {
            // Move towards target position
            navAgent.SetDestination(point.position);

            // Turn the attacker to face the opponent
            transform.LookAt(point.position);
        }
    }
    #endregion
}
