using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Firing : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    private Targetting targetting;
    private GameObject target;
    
    
    // Rate at which you fire
    [SerializeField] private float rps = 1f;

    private float range; 
    private float timeLastFired;

    public override void OnStartServer()
    {
        // Get the targetting script
        targetting = this.GetComponent<Targetting>();

        // Get the range of shooting based off targetting
        range = targetting.GetTargetRange();
    }

    [ServerCallback] private void Update()
    {
        target = targetting.GetTarget();

        if (!FireAtAble()) {return;}

        // Only fire if there has been enough time since the last fire time
        if(Time.time > (1 / rps) + timeLastFired)
        {
            // Create the bullet
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, gameObject.transform.rotation);
            
            // Set the target for the bullet
            bullet.GetComponent<BulletMovement>().SetTarget(targetting.GetTarget());

            // Spawn the bullet over the network
            NetworkServer.Spawn(bullet, connectionToClient);

            // After firing update the time last fired to be the current time
            timeLastFired = Time.time;
        }
    }

    [Server] private bool FireAtAble()
    {
        // If there is no target then leave this function
        if(target == null) {return false;}
         
        // Return whether the target is within fire range
        return (targetting.GetTarget().transform.position - transform.position).sqrMagnitude 
            < range * range; // Keep both values squared to reduce processing time
        
    }
}
