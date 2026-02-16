using EZCameraShake;
using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class slamming : MonoBehaviour, Abilities
{
    public bool unlocked { get; set; }
    public bool hasEnergyEffect { get; set; }

    public float slammingPower, slamDamage, slamRadius, slamForce;

    public float empDamage, empRadius, empForce;

    public GameObject slamPartical;

    playerMovement movement;

    public bool Slamming, empSlam;

    public GameObject empPartical;

    public LayerMask enemyLayer;

    public TrailRenderer TrailRenderer;
    void Start()
    {
        movement = GetComponent<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(!movement.grounded())
            {
                Slamming = true;
            }
        }
        if (movement.grounded())
        {
            Slamming = false;
        }
        if (movement.dashing)
        {
            Slamming = false;
        }

        if (Slamming)
        {
            movement.rb.velocity = new Vector3(0, slammingPower, 0);
        }


        TrailRenderer.enabled = Slamming;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Slamming)
        {
            if(!empSlam)
            {
                Collider[] hits = Physics.OverlapSphere(transform.position, slamRadius, enemyLayer);

                CameraShaker.Instance.ShakeOnce(5, 10, 0, 0.45f);

                foreach (Collider col in hits)
                {
                    EnemyHealth enemyHealth = col.GetComponent<EnemyHealth>();

                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(collision.GetContact(0).point, slamDamage);
                    }

                    var rb = col.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.constraints = RigidbodyConstraints.None;

                        rb.AddForce(Vector3.up * slamForce, ForceMode.VelocityChange);
                    }
                }
                GameObject GO = Instantiate(slamPartical, transform.position, Quaternion.identity);
                Destroy(GO, 3);
                SoundManager.PlaySound(SoundType.slam);
            }
            else
            {
                Collider[] hits = Physics.OverlapSphere(transform.position, empRadius, enemyLayer);

                CameraShaker.Instance.ShakeOnce(10, 10, 0, 0.45f);

                foreach (Collider col in hits)
                {
                    EnemyHealth enemyHealth = col.GetComponent<EnemyHealth>();

                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(collision.GetContact(0).point, empDamage);
                        enemyHealth.GetStunned(4);
                    }

                    var rb = col.GetComponent<Rigidbody>();
                    if(rb != null)
                    {
                        rb.AddExplosionForce(empForce, transform.position, empRadius, 1);
                    }                   
                }
                SoundManager.PlaySound(SoundType.empSlam);
                GameObject GO = Instantiate(empPartical, transform.position, Quaternion.identity);
                Destroy(GO, 3);
            }          
        }
    }

    public void upgrade(UpGradeSO UPGSO)
    {
        if (UPGSO.weaponID != 4) { return; }

        if (UPGSO.upGradeID == 0) 
        {
            empSlam = true;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, slamRadius);
        Gizmos.color = Color.cyan; 
        Gizmos.DrawWireSphere(transform.position, empRadius);

    }
}
