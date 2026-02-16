using SmallHedge.SoundManager;
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
    public AudioSource chainLoop;
    [Header("throw")]
    public bool thrown = false;
    public GameObject _object;
    public bool canThrow;
    public float Power = 100, cooldown, cooldownTimer, energyCooldown;
    [Header("chain saw")]
    public float SpinSpeed = 60, damage;
    public float lifeTime = 3;

    public bool unlocked { get; set; }
    public bool hasEnergyEffect { get; set; }
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

    public void EnergyEffect(float duration)
    {
        hasEnergyEffect = true;

        Invoke(nameof(resetEffect), duration);
    }

    void resetEffect()
    {
        hasEnergyEffect = false;
    }

    private void Update()
    {       
        spinStuff();
        
        if (!thrown && unlocked)
        {           
            icon.maxValue = cooldown;
            if (hasEnergyEffect)
            {
                icon.value = icon.maxValue;
            }
            else
            {
                icon.value = cooldownTimer;
            }
            

            if (Input.GetKeyDown(KeyCode.E) && canThrow && PlayerHealth.instance.alive)
            {
                chainLoop.Play();
            }

            if (Input.GetKey(KeyCode.E) && canThrow && PlayerHealth.instance.alive)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].enabled = true;
                }
                chainLoop.pitch += Time.deltaTime * 0.7f;
                models.gameObject.SetActive(true);
            }

            if (Input.GetKeyUp(KeyCode.E) && canThrow)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].enabled = false;
                }
                chainLoop.pitch = 0;
                models.gameObject.SetActive(false);
                ThrowObj(_object, Power);
                cooldownTimer = 0;
                canThrow = false;
                chainLoop.Stop();
            }

            if (hasEnergyEffect)
            {
                if (cooldownTimer >= energyCooldown)
                {
                    canThrow = true;
                }
                else
                {
                    cooldownTimer += Time.deltaTime;
                }
            }
            else
            {
                if (cooldownTimer >= cooldown)
                {
                    canThrow = true;
                }
                else
                {
                    cooldownTimer += Time.deltaTime;
                }
            }
            chainLoop.pitch = Mathf.Clamp(chainLoop.pitch, 0, 1f);

        }
    }

    void ThrowObj(GameObject obj, float ThrowPower)
    {
        GameObject GO = Instantiate(obj, transform.position, Camera.main.transform.rotation);

        Rigidbody RB = GO.GetComponent<Rigidbody>();

        RB.AddForce(GO.transform.forward * ThrowPower, ForceMode.Impulse);

        GO.GetComponent<chainSaw>().damage = damage * 1.2f;
        GO.GetComponent<AudioSource>().Play();
    }

    [ContextMenu("UNLOCK ME YOU SICK FU")]
    void unlock()
    {
        unlocked = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        SoundManager.PlaySound(SoundType.chainsawHit);
        EnemyHealth EN = other.GetComponent<EnemyHealth>();

        RaycastHit hit;

        Vector3 direction = (other.transform.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, direction, out hit, 5f))
        {
            if (EN != null)
            {
                EN.TakeDamage(hit.point, damage);
            }
        }      
    }


    void spinStuff()
    {
        if (!thrown)
        {
            transform.position = Player.position;
            icon.gameObject.SetActive(unlocked);
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
        if (UPGSO.upGradeID == 3)
        {
            unlocked = true;
        }
    }
}
