using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EnemyTurn : MonoBehaviour
{
    public float lookSpeed;
    public Vector3 offset;
    Rigidbody rb;
    [Header("lock")]
    public bool lockUp;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerMovement.instance.transform != null)
        {
            Vector3 playerPosition = playerMovement.instance.transform.position + offset;
            Vector3 lookDirection = playerPosition - transform.position;

            Vector3 flatDir = new Vector3(lookDirection.x, 0, lookDirection.z);

            if (rb != null)
            {
                if (lockUp)
                {
                    rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(flatDir), lookSpeed));
                }
                else
                {
                    rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), lookSpeed));
                }
            }
            else
            {
                if (lockUp)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(flatDir), lookSpeed);
                }
                else
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), lookSpeed);
                }
            }
            
        }

    }
}
