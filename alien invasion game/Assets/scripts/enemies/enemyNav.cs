using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;

public class enemyNav : MonoBehaviour
{
    [HideInInspector]public Transform target;
    public float speed, groundDetectionDistance, groundDrag;
    public Transform detectionOrigin;
    public LayerMask ground;

    Rigidbody rb;

    void Awake()
    {       
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        speed += scoreManager.instance.currentWave * 6f;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!GetComponent<EnemyHealth>().stuned && Grounded())
        {
            Vector3 dir = (target.position - transform.position).normalized;
            Vector3 flatDir = new Vector3(dir.x, 0, dir.z);
            rb.AddForce(flatDir * speed * 10 * Time.fixedDeltaTime, ForceMode.Force);
            rb.constraints = RigidbodyConstraints.FreezeRotationX;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
            rb.drag = groundDrag;
            rb.useGravity = false;
        }
        else
        {
            rb.drag = 0;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
            rb.useGravity = true;
        }

        
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (detectionOrigin == null)
        {
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundDetectionDistance);
        }
        else
        {
            Gizmos.DrawLine(detectionOrigin.position, detectionOrigin.position + Vector3.down * groundDetectionDistance);
        }
        
    }
    public bool Grounded()
    {
        if(detectionOrigin == null)
        {
            return Physics.Raycast(transform.position, Vector3.down, groundDetectionDistance, ground);
        }
        else
        {
            return Physics.Raycast(detectionOrigin.position, Vector3.down, groundDetectionDistance, ground);
        }

    }
}
