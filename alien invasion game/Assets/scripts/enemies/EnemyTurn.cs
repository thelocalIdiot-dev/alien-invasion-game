using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EnemyTurn : MonoBehaviour
{
    public float lookSpeed;
    public Vector3 offset;
    [Header("lock")]
    public bool lockUp;

    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerMovement.instance.transform != null)
        {
            Vector3 playerPosition = playerMovement.instance.transform.position + offset;
            Vector3 lookDirection = playerPosition - transform.position;

          

            if(lockUp)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(lookDirection.x, 0, lookDirection.z)), lookSpeed);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), lookSpeed);
            }
        }

    }
}
