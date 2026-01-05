using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;

[RequireComponent(typeof(NavMeshAgent))]
public class enemyNav : MonoBehaviour
{
    public Transform target;
    int updateEveryNFrames = 5;

    private NavMeshAgent agent;
    private int frameCounter;

    void Awake()
    {       
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {       
        if (target != null)
        {
            frameCounter++;

            if (frameCounter >= updateEveryNFrames)
            {
                agent.SetDestination(target.position);
                frameCounter = 0;
            }
        }
       
    }
}
