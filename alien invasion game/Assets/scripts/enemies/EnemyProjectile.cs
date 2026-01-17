using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("enemy")]
    public float lookSpeed;
    public Vector3 offset;
    public int numberOfBullets = 1;
    public Transform Bone;
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

    public Vector2 spread;

    void Start()
    {
        shootCooldownTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {        
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

    private void LateUpdate()
    {
        //lookTowards();
    }

    void shoot()
    {
        int bullets = Mathf.Max(1, numberOfBullets);

        for (int i = 0; i < bullets; i++)
        {
            float t = (bullets == 1) ? 0.5f : (float)i / (bullets - 1);

            float horizontalAngle = Mathf.Lerp(-spread.x / 2f, spread.x / 2f, t);
            float verticalAngle = Mathf.Lerp(-spread.y / 2f, spread.y / 2f, t);

            Quaternion spreadRotation =
                firePoint.rotation * Quaternion.Euler(verticalAngle, horizontalAngle, 0f);

            GameObject bullet = Instantiate(projectile, firePoint.position, spreadRotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = spreadRotation * Vector3.forward * bulletSpeed + offset;

            EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
            bulletScript.damage = ProjectileDamage;
        }
    }


    void lookTowards()
    {
        if(playerMovement.instance.transform != null)
        {
            Vector3 playerPosition = playerMovement.instance.transform.position + offset;
            Vector3 lookDirection = playerPosition - transform.position;
            Bone.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), lookSpeed);
        }
        
    }
}
