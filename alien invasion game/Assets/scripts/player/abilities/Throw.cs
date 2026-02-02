using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throw : MonoBehaviour, Abilities
{
    public GameObject _object;
    public bool canThrow;
    public float basePower, MaxPower = 100, cooldown, cooldownTimer;

    public bool unlocked { get; set; }

    float BaseDamage, BaseRadius;

    public KeyCode throwKey = KeyCode.Mouse1;

    public Slider icon;
    public Slider chargeIcon;

    private void Start()
    {
        BaseDamage = _object.GetComponent<bomb>().explosion.damage;
        BaseRadius = _object.GetComponent<bomb>().explosion.radius;
        canThrow = true;
    }

    // Update is called once per frame
    void Update()
    {
        chargeIcon.maxValue = MaxPower;
        chargeIcon.value = basePower;

        icon.gameObject.SetActive(unlocked);
        if (!unlocked) return;
        icon.maxValue = cooldown;
        icon.value = cooldownTimer;

        if (Input.GetKey(throwKey) && canThrow && PlayerHealth.instance.alive)
        {
            basePower += Time.deltaTime * 50;
        }

        basePower = Mathf.Clamp(basePower, 0, MaxPower);

        if (Input.GetKeyUp(throwKey) && canThrow)
        {
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

    void ThrowObj(GameObject obj, float ThrowPower)
    {
        GameObject GO = Instantiate(obj, transform.position, transform.rotation);

        Rigidbody RB = GO.GetComponent<Rigidbody>();

        RB.AddForce(GO.transform.forward * ThrowPower, ForceMode.Impulse);
    }

    [ContextMenu("UNLOCK ME YOU SICK FU")]
    void unlock()
    {
        unlocked = true;
    }

    public void upgrade(UpGradeSO UPGSO)
    {
        if (UPGSO.weaponID != 1) { return; }

        if (UPGSO.upGradeID == 0)
        {
            _object.GetComponent<bomb>().explosion.damage *= UPGSO.upGradeAmount;
        }
        if (UPGSO.upGradeID == 1)
        {
            cooldown *= UPGSO.upGradeAmount;
        }
        if (UPGSO.upGradeID == 2)
        {
            _object.GetComponent<bomb>().explosion.radius *= UPGSO.upGradeAmount;
            _object.GetComponent<bomb>().explosion.partical.GetComponent<ParticleSystem>().startSize *= UPGSO.upGradeAmount;
        }
        if (UPGSO.upGradeID == 3)
        {
            unlocked = true;
        }
    }

    private void OnDisable()
    {
        _object.GetComponent<bomb>().explosion.damage = BaseDamage;
        _object.GetComponent<bomb>().explosion.radius = BaseRadius;
        _object.GetComponent<bomb>().explosion.partical.GetComponent<ParticleSystem>().startSize = BaseRadius * 2;
    }
}
