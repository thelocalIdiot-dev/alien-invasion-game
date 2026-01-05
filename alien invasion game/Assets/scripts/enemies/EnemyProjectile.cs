using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("enemy")]
    public float lookSpeed;

    [Header("references")]
    public Animator animator;
    public GameObject projectile;
    public Transform firePoint;

    [Header("projectile")]       
    public float bulletSpeed = 10;
    public float shootCooldown = 1;
    public float shootDelay;
    public float ProjectileDamage;
    float shootCooldownTimer;

    void Start()
    {
        shootCooldownTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        lookTowards();

        if(shootCooldownTimer >= shootCooldown)
        {
            animator.SetTrigger("shoot");
            Invoke(nameof(shoot), shootDelay);            
            shootCooldownTimer = 0;
        }
        else
        {
            shootCooldownTimer += Time.deltaTime;
        }
    }

    void shoot()
    {
        GameObject bullet = Instantiate(projectile, firePoint.position, transform.rotation);
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();       
        bulletRB.velocity = transform.forward * bulletSpeed;
        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
        bulletScript.damage = ProjectileDamage;
    }

    void lookTowards()
    {
        Vector3 playerPosition = playerMovement.instance.transform.position;
        Vector3 lookDirection = playerPosition - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), lookSpeed);
    }
}
