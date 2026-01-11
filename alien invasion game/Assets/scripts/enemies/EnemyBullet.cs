using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject impact;
    [HideInInspector] public float damage;
    public LayerMask IgnoreLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == IgnoreLayer)
        {
            Debug.Log(other.name);
            return;
        }
        GameObject impactGO = Instantiate(impact, transform.position, Quaternion.identity);
        Destroy(impactGO, 0.5f);

        PlayerHealth PH = other.GetComponent<PlayerHealth>();

        if (PH != null)
        {
            PH.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
