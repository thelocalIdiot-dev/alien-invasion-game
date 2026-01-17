using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class chainSaw : MonoBehaviour
{
    [Header("references")]
    Transform Player;
    Collider[] colliders;
    public Transform models;
    public Slider icon;
    [Header("throw")]
    public bool thrown = false;
    public GameObject _object;
    public bool canThrow;
    public float basePower, MaxPower = 100, cooldown, cooldownTimer;
    [Header("chain saw")]
    public float SpinSpeed = 60, damage;
    public int maxHit = 3;
    public int currentHitCount;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        colliders = GetComponents<Collider>();
    }

    private void Update()
    {
        spinStuff();
        icon.value = Mathf.Clamp(cooldownTimer, 0, cooldownTimer/cooldown);
        if (!thrown)
        {
            if (Input.GetKey(KeyCode.E) && canThrow)
            {
                basePower += Time.deltaTime * 50;
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].enabled = true;
                }
                models.gameObject.SetActive(true);
            }

            basePower = Mathf.Clamp(basePower, 10, MaxPower);

            if (Input.GetKeyUp(KeyCode.E) && canThrow)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].enabled = false;
                }
                models.gameObject.SetActive(false);
                ThrowObj(_object, basePower);
                cooldownTimer = 0;
                canThrow = false;
                basePower = 0;
            }

            if (cooldownTimer >= cooldown)
            {
                canThrow = true;
            }
            else
            {
                cooldownTimer += Time.deltaTime;
            }
        }        
    }

    void ThrowObj(GameObject obj, float ThrowPower)
    {
        GameObject GO = Instantiate(obj, transform.position, Camera.main.transform.rotation);

        Rigidbody RB = GO.GetComponent<Rigidbody>();

        RB.AddForce(GO.transform.forward * ThrowPower, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth EN = other.GetComponent<EnemyHealth>();
        
        if (EN != null)
        {
            EN.TakeDamage(damage);
        }
    }

    void spinStuff()
    {
        if (!thrown)
        {
            transform.position = Player.position;
        }

        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, SpinSpeed, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (thrown)
        {
            currentHitCount++;

            if(currentHitCount > maxHit)
            {
                Destroy(gameObject);
            }
        }
    }

}
