using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrab : MonoBehaviour
{
    public float grabDistance, grabDelay, grabCooldown, explodeTime, explodeTimer;
    public Transform grabPoint;
    public bool grabbing = false, canGrab = true;
    public Animator animator;
    public explosionObj explosion;
    public float FlashDelay, FlashDelayTimer;
    public bool canExplode;
    private void Update()
    {
        float Distance = Vector3.Distance(playerMovement.instance.transform.position, grabPoint.position);
        if(Distance < grabDistance && !grabbing && canGrab)
        {
            Invoke(nameof(grab), grabDelay);
            grabbing = true;
            canGrab = false;
        }

        animator.SetBool("Grabbing", grabbing);

        if (grabbing) { canExplode = true; }

        if (canExplode)
        {
            if (explodeTimer > explodeTime)
            {
                explosionManager.Explosion(grabPoint.position, explosion);

                Destroy(gameObject);
            }
            else
            {
                if(FlashDelayTimer > FlashDelay)
                {
                    GetComponent<EnemyHealth>().Flash();
                    FlashDelayTimer = 0;
                    FlashDelay /= 2;
                }
                else
                {
                    FlashDelayTimer += Time.deltaTime;
                }
                
                if(FlashDelay < 0) { FlashDelay = 0.07f; }

                explodeTimer += Time.deltaTime;
            }
        }
        
    }   

    void grab()
    {
        Collider[] player = Physics.OverlapBox(grabPoint.position, new Vector3(grabDistance, grabDistance, grabDistance));

        foreach (Collider col in player)
        {
            explodeTimer = 0;

            playerMovement.instance.Lock(GetComponent<EnemyGrab>());
            playerMovement.instance.transform.position = grabPoint.position;
        }
    }

    public void release()
    {
        grabbing = false;
        Debug.Log("iuh");
        Invoke(nameof(resetGrab), grabCooldown);
    }

    void resetGrab()
    {
        canGrab = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(grabPoint.position, new Vector3(grabDistance, grabDistance, grabDistance));
    }

    private void OnDestroy()
    {
        Collider[] player = Physics.OverlapBox(grabPoint.position, new Vector3(grabDistance, grabDistance, grabDistance));

        foreach (Collider col in player)
        {
            if (grabbing)
            {
                playerMovement.instance.breakFree();
            }
                        
        }
    }

}
