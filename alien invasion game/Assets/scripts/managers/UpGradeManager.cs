using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class UpGradeManager : MonoBehaviour
{
    public GameObject[] abilitiesOBJ;
    public Transform[] buttonSlots;

    public GameObject buttonTemplate;

    public UpGradeSO[] upgrades;

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
        }       
    }

    public void setUpButton()
    {
        for (int i = 0;i < buttonSlots.Length;i++)
        {
            Instantiate(buttonTemplate, buttonSlots[i]);
        }
    }
}
