using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healDrop : MonoBehaviour
{
    Transform player;
    Rigidbody rb;
    public float speed = 10;
    public float damage;
    float lifetime;
    public float XPvalue;
    public GameObject effect;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        lifetime = speed;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime += Time.deltaTime;
        Vector3 dir = player.position + Vector3.up - transform.position;
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 8000));        
       
        rb.velocity = transform.forward * speed * lifetime;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth EH = other.GetComponent<EnemyHealth>();

        if (EH != null && valueManager.instance.XPdropDamage)
        {
            GameObject effects = Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(effects, 1);
            EH.TakeDamage(damage);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            GameObject effects = Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(effects, 1);
            scoreManager.instance.UpdateXp(XPvalue);
            PlayerHealth.instance.currentHealth += valueManager.instance.lifeSteal;
            Destroy(gameObject);
        }      
    }

}
