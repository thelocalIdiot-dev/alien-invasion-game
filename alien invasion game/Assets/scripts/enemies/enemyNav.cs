using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;

public class enemyNav : MonoBehaviour
{
    [HideInInspector]public Transform target;
    public float speed;

    void Awake()
    {       
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        Vector3 dir = target.position - transform.position;
        Vector3 flatDir = new Vector3(dir.x, 0, dir.z);
        Vector3 nextPos = transform.position + flatDir * speed * Time.fixedDeltaTime;
        GetComponent<Rigidbody>().MovePosition(nextPos);
    }
}
