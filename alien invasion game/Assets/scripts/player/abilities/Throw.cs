using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throw : MonoBehaviour
{
    public GameObject _object;
    public bool canThrow;
    public float basePower, MaxPower = 100, cooldown, cooldownTimer;

    public KeyCode throwKey = KeyCode.Mouse1;

    public Slider icon;

    private void Start()
    {
        canThrow = true;
    }

    // Update is called once per frame
    void Update()
    {
        icon.value = Mathf.Clamp(cooldownTimer, 0, cooldownTimer / cooldown);

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
}
