using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectOrb : MonoBehaviour
{
    public float duration;
    public GameObject particals;
    public GameObject[] abilityObj;

    public Color flashColor;

    Abilities[] abilities;
    public enum types { speed, energy}

    public types Type;

    public void Awake()
    {
        abilityObj = GameObject.FindGameObjectsWithTag("ability");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {          
            if(Type == types.speed)
            {
                playerMovement.instance.SpeedEffect(duration);
                screenFlash.instance.ScreenFlash(flashColor, 0.4f);

                GameObject particalObject = Instantiate(particals, transform.position, Quaternion.identity);
                Destroy(particalObject, 0.6f);
                Destroy(gameObject);
            }
            else if (Type == types.energy)
            {
                for (int i = 0; i < abilityObj.Length; i++)
                {
                    abilityObj[i].GetComponent<Abilities>().EnergyEffect(duration);                   
                }

                GameObject particalObject = Instantiate(particals, transform.position, Quaternion.identity);
                Destroy(particalObject, 0.6f);
                Destroy(gameObject);
            }
            
        }
    }

}
