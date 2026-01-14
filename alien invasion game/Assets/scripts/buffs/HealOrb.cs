using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOrb : MonoBehaviour
{
    public float ammoReload, healAmount;
    public GameObject particals;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().currentHealth += healAmount;
            other.GetComponent<gun>().bulletsLeft += ammoReload;

            GameObject particalObject = Instantiate(particals, transform.position, Quaternion.identity);
            Destroy(particalObject, 0.6f);
            Destroy(gameObject);
        }
    }
}
