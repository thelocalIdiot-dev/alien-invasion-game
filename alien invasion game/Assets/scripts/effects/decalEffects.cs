using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class decalEffects : MonoBehaviour
{
    public GameObject[] decals;
    public float decalPropability;
    List<ParticleCollisionEvent> events;

    public LayerMask layer;

    int random, randDecal;

    public void Awake()
    {
        Transform detectionPlane = GameObject.FindGameObjectWithTag("ground").GetComponent<Transform>();
        GetComponent<ParticleSystem>().collision.AddPlane(detectionPlane);
        GetComponent<ParticleSystem>().collision.GetPlane(0).transform.localScale= new Vector3(10000f, 0f, 10000f);        
    }

    private void OnParticleCollision(GameObject other)
    {
        random = Random.Range(0, 100);
        Debug.Log("gdagdigdagdaohilbmariedalongtamago");
        if (random > decalPropability)
        {           
            randDecal = Random.Range(0, decals.Length);
            Vector3 position = events[Random.Range(0, events.Count)].intersection;
            Vector3 normal = events[Random.Range(0, events.Count)].normal;
            Instantiate(decals[randDecal], position, Quaternion.LookRotation(normal));
        }
    }
}
