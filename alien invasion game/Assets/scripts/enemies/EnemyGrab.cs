using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrab : MonoBehaviour
{
    public float grabDistance, grabDelay, grabCooldown, explodeTime, explodeTimer;
    public Transform grabPoint;
    public bool grabbing = false, canGrab = true, canExplode;
    public Animator animator;
    public explosionObj explosion;
    public float FlashDelay, FlashDelayTimer;
    public GameObject warning;

    public float _flashDelay;

    public void Awake()
    {
        _flashDelay = FlashDelay;
    }

    private void Update()
    {
        float Distance = Vector3.Distance(playerMovement.instance.transform.position, grabPoint.position);
        if(Distance < grabDistance && !grabbing && canGrab)
        {
            Invoke(nameof(grab), grabDelay);
            GameObject warningObj = Instantiate(warning, transform.position, Quaternion.identity);
            Destroy(warningObj, 1);
            grabbing = true;
            
        }

        animator.SetBool("Grabbing", grabbing);

        if (canExplode)
        {
            if (explodeTimer > explodeTime)
            {
                explosionManager.Explosion(grabPoint.position, explosion);

                Destroy(gameObject);
            }
            else
            {
                if(FlashDelayTimer > _flashDelay)
                {
                    GetComponent<EnemyHealth>().Flash();
                    SoundManager.PlaySound(SoundType.beep);
                    FlashDelayTimer = 0;
                    _flashDelay /= 1.36f;
                }
                else
                {
                    FlashDelayTimer += Time.deltaTime;
                }
                
                if(_flashDelay < 0) { _flashDelay = 0.09f; }

                explodeTimer += Time.deltaTime;
            }
        }
        
    }   

    void grab()
    {
        Collider[] player = Physics.OverlapBox(grabPoint.position, new Vector3(grabDistance, grabDistance, grabDistance));

        foreach (Collider col in player)
        {
            float Distance = Vector3.Distance(playerMovement.instance.transform.position, grabPoint.position);
            if (Distance < grabDistance)
            {
                explodeTimer = 0;
                _flashDelay = FlashDelay;
                canExplode = true;
                playerMovement.instance.Lock(GetComponent<EnemyGrab>());
                playerMovement.instance.transform.position = grabPoint.position;
            }
            else
            {
                grabbing = false;
                canGrab = true;
                canExplode = false;
            }           
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
