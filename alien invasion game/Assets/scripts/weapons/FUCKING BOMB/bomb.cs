using EZCameraShake;
using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public explosionObj explosion;
    
    //private float _explosionForce = 700;

    //public LayerMask IgnoreLayer;
    private void OnTriggerEnter(Collider other)
    {
        explode(explosion);
    }

    public void explode(explosionObj EO)
    {

        explosionManager.Explosion(transform.position, EO);     
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, explosion.radius);
    }
}
