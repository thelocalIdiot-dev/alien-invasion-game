using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class chainSaw : MonoBehaviour, Abilities
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
    public float Power = 100, cooldown, cooldownTimer;
    [Header("chain saw")]
    public float SpinSpeed = 60, damage;
    public float lifeTime = 3;


    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        colliders = GetComponents<Collider>();       

        if (thrown)
        {
            Destroy(gameObject, lifeTime);
        }
        else
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = false;
            }
            models.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        spinStuff();
        
        if (!thrown)
        {
            icon.maxValue = cooldown;
            icon.value = cooldownTimer;

            if (Input.GetKey(KeyCode.E) && canThrow)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].enabled = true;
                }
                models.gameObject.SetActive(true);
            }

            if (Input.GetKeyUp(KeyCode.E) && canThrow)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].enabled = false;
                }
                models.gameObject.SetActive(false);
                ThrowObj(_object, Power);
                cooldownTimer = 0;
                canThrow = false;
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

    private void OnCollisionEnter(Collision collision)
    {
        EnemyHealth EN = collision.gameObject.GetComponent<EnemyHealth>();

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

    public void upgrade(UpGradeSO UPGSO)
    {
        if (UPGSO.weaponID != 2) { return; }

        if (UPGSO.upGradeID == 0)
        {
            damage *= UPGSO.upGradeAmount;
        }
        if (UPGSO.upGradeID == 1)
        {
            cooldown *= UPGSO.upGradeAmount;
        }
        if (UPGSO.upGradeID == 2)
        {
            Power *= UPGSO.upGradeAmount;
        }
    }
}
