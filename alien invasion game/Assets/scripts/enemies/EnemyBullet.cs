using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float steeringPower = 0;
    public GameObject impact;
    [HideInInspector] public float damage;
    public LayerMask IgnoreLayer;
    Transform player;
    Rigidbody rb;
    public float speed;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        speed = rb.velocity.magnitude;
    }

    private void Update()
    {       
        Vector3 dir = player.position - transform.position;
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime *  steeringPower));
        rb.velocity = transform.forward * speed;
    }

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

        SoundManager.PlaySound(SoundType.projectileHit);
        Destroy(gameObject);
    }
}
