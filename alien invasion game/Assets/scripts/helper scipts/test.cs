using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Transform target;

    public float decrease;

    public float Time;
    void Update()
    {
        Vector3 desiredPos = target.position + playerMovement.instance.rb.velocity * decrease;

        float X = Mathf.Lerp(transform.position.x, desiredPos.x, Time);
        float Y = Mathf.Lerp(transform.position.y, desiredPos.y, Time);
        float Z = Mathf.Lerp(transform.position.z, desiredPos.z, Time);

        Vector3 pos = new Vector3(X, Y, Z);

        transform.position = pos;

        transform.LookAt(target.position);
    }
}
