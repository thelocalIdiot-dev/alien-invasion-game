using EZCameraShake;
using SmallHedge.SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class explosionManager : MonoBehaviour
{

    public static void Explosion(Vector3 position, explosionObj explosion)
    {
        var surroundingObjects = Physics.OverlapSphere(position, explosion.radius);

        

        // effects
        SoundManager.PlaySound(SoundType.explosion);
        CameraShaker.Instance.ShakeOnce(explosion.mag, explosion.rough, 0, explosion.fadeOut);

        foreach (var obj in surroundingObjects)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosion.BlastForce, position, explosion.radius, 1);
            }
            
            if(explosion.player)
            {
                var Healths = obj.GetComponent<PlayerHealth>();
                if (Healths != null) { Healths.TakeDamage(explosion.damage); }
            }
            if(explosion.enemy)
            {
                var Healths = obj.GetComponent<EnemyHealth>();
                if (Healths != null) { Healths.TakeDamage(explosion.damage); }
            }
                        
        }

        GameObject explosionObj = Instantiate(explosion.partical, position, Quaternion.identity);

        Destroy(explosionObj, 1);
    }

}
  
   
  


