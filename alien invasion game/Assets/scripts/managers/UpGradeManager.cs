using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpGradeManager : MonoBehaviour
{
    public GameObject[] abilitiesOBJ;
    public Transform[] buttonSlots;

    public GameObject buttonTemplate;

    public List<UpGradeSO> upgrades;

    public static UpGradeManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void UpGrade(UpGradeSO upgradeSO)
    {
        for (int i = 0; i < abilitiesOBJ.Length; i++)
        {
            abilitiesOBJ[i].GetComponent<Abilities>().upgrade(upgradeSO);
            if (!upgradeSO.stackable)
            {
                upgrades.Remove(upgradeSO);
            }
        }       
    }

    public UpGradeSO getRandomSO()
    {
        int random = Random.Range(0, upgrades.Count);
        UpGradeSO randomSO = upgrades[random];
        if (randomSO.requiresUnlock)
        {
            if (!abilitiesOBJ[randomSO.weaponID].GetComponent<Abilities>().unlocked)
            {
                return getRandomSO();
            }
        }
        return randomSO;
    }

    public void setUpButton()
    {
        for (int i = 0;i < buttonSlots.Length;i++)
        {
            Instantiate(buttonTemplate, buttonSlots[i]);
        }
    }
}
