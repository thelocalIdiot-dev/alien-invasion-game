using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healDrop : MonoBehaviour
{
    Transform player;
    Rigidbody rb;
    public float speed = 10;
    float lifetime;
    public float value;
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
        Vector3 dir = player.position - transform.position;
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10000));        
       
        rb.velocity = transform.forward * speed * lifetime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject effects = Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(effects, 1);
            scoreManager.instance.UpdateXp(value);
            Destroy(gameObject);
        }      
    }
}
