using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BulletMovement : NetworkBehaviour
{
    
    [SerializeField] private float shotSpeed = 10f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifeTime = 5f;
    
    private GameObject target = null;

    
    // When this object collides with a unit
    [ServerCallback] private void OnTriggerEnter(Collider hit)
    {
        #region Exit Conditions

        // Don't interact with objects also owned by you
        if(hit.gameObject.GetComponent<NetworkIdentity>().connectionToClient.connectionId == 
            gameObject.GetComponent<NetworkIdentity>().connectionToClient.connectionId) {return;}
        
        // Don't interact with anything that isn't targettable
        if (hit.gameObject.layer != LayerMask.NameToLayer("Targettable")){return;}
        
        // Only interact with cube colliders
        if (hit.GetType() != typeof(BoxCollider)){return;}

        #endregion

        // Check collider has damage component and if it does assign it to the damageComp variable
        if(hit.TryGetComponent<Damage>(out Damage damageComp))
        {
            // Call the public do damage function
            damageComp.DoDamage(this.damage);
        }
        
        // Destroy self projectile
        SelfDestruct();
    
    }

    // Set the target for the projectile
    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    public override void OnStartServer()
    {
        // Invoke the given function after a given amount of time
        Invoke(nameof(SelfDestruct), lifeTime);
    }
    [ServerCallback] private void Update()
    {   
        if(target)
        {
            // Move the bullet towards the target
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, shotSpeed);
        }
        else 
        {
            // If the bullet doesn't have a target delete itself
            SelfDestruct();
        }
    }
    [Server] private void SelfDestruct()
    {
        // Destroys this game object for all clients
        NetworkServer.Destroy(gameObject);
    }
    
}
