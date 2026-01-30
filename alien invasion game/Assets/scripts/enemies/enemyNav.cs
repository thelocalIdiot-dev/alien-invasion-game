using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;

public class enemyNav : MonoBehaviour
{
    [HideInInspector]public Transform target;
    public float speed, groundDetectionDistance;

    

    void Awake()
    {       
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        speed *= scoreManager.instance.currentWave * 0.55f;
    }

    private void FixedUpdate()
    {
        if (!GetComponent<EnemyHealth>().stuned)
        {
            Vector3 dir = target.position - transform.position;
            Vector3 flatDir = new Vector3(dir.x, 0, dir.z);
            Vector3 nextPos = transform.position + flatDir * speed * Time.fixedDeltaTime;
            GetComponent<Rigidbody>().MovePosition(nextPos);
        }
        
        
    }

    public bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundDetectionDistance);
    }
}
