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

    float BaseDamage;

    public KeyCode throwKey = KeyCode.Mouse1;

    public Slider icon;

    private void Start()
    {
        BaseDamage = _object.GetComponent<bomb>().explosion.damage;
        canThrow = true;
    }

    // Update is called once per frame
    void Update()
    {
        icon.gameObject.SetActive(unlocked);
        if (!unlocked) return;
        icon.maxValue = cooldown;
        icon.value = cooldownTimer;

        if (Input.GetKey(throwKey) && canThrow)
        {
            basePower += Time.deltaTime * 50;
        }

        basePower = Mathf.Clamp(basePower, 10, MaxPower);

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
            unlocked = true;
        }
    }

    private void OnDisable()
    {
        _object.GetComponent<bomb>().explosion.damage = BaseDamage;
    }
}
