using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplode : MonoBehaviour
{
    public explosionObj explosion;
    public float explodeDistance;

    public GameObject warning;

    public Animator animator;

    public float explodeDelay;

    public bool canExplode = true;
    void Update()
    {
        float Distance = Vector3.Distance(playerMovement.instance.transform.position, transform.position);

        if(Distance < explodeDistance && canExplode && !GetComponent<EnemyHealth>().stuned)
        {
            GameObject GO = Instantiate(warning, transform.position, Quaternion.identity);
            Destroy(GO, 2);
            animator.SetTrigger("explode");
            SoundManager.PlaySound(SoundType.beep);
            Invoke(nameof(explode), explodeDelay); 
            canExplode = false;
            GetComponent<enemyNav>().enabled = false;
        }
    }

    void explode()
    {
        explosionManager.Explosion(transform.position, explosion);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explodeDistance);
    }
}
