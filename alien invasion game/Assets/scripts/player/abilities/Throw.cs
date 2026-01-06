using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public GameObject _object;
    public bool canThrow;
    public float power = 100, cooldown, cooldownTimer;

    public KeyCode throwKey = KeyCode.Mouse1;

    private void Start()
    {
        canThrow = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(throwKey) && canThrow)
        {
            ThrowObj(_object);
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

    void ThrowObj(GameObject obj)
    {
        GameObject GO = Instantiate(obj, transform.position, transform.rotation);

        Rigidbody RB = GO.GetComponent<Rigidbody>();

        RB.AddForce(GO.transform.forward * power, ForceMode.Impulse);

        RB.angularVelocity = new Vector3(0, 10, 0);
    }
}
