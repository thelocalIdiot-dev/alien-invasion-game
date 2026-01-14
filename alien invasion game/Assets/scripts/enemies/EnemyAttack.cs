using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    enemyNav EN;
    public Animator animator;
    public float attackDistance, attackDelay, attackCooldown, damage;
    public bool canAttack;
    public bool attacked;
    public SoundType attackSound;
    
    // Start is called before the first frame update
    void Awake()
    {
        attacked = false;
        canAttack = true;
        EN = GetComponent<enemyNav>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EN.target != null)
        {
            float distance = Vector3.Distance(transform.position, EN.target.position);

            if (distance < attackDistance && canAttack && attacked == false)
            {
                animator.SetTrigger("attack");
                Invoke(nameof(attack), attackDelay);
                Invoke(nameof(resetAttack), attackCooldown);
                attacked = true;
                canAttack = false;
            }
        }
        
    }

    void attack()
    {
        SoundManager.PlaySound(attackSound);

        Collider[] hits = Physics.OverlapSphere(transform.position, attackDistance);

        foreach (Collider hit in hits)
        {
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        
        
    }

    void resetAttack()
    {
        attacked = false;
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
