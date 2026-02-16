using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class valueManager : MonoBehaviour, Abilities
{
    public bool unlocked { get; set; }
    public bool hasEnergyEffect { get; set; }

    public static valueManager instance;

    public float extraOrb = 0;
    public float lifeSteal = 1;
    public float SpeedMultiplier = 1;
    public float defence = 1;
    public bool XPdropDamage = false;
    void Start()
    {
        unlocked = true;
    }

    void Awake()
    {
        instance = this;
    }


    public void upgrade(UpGradeSO UPGSO)
    {
        if (UPGSO.weaponID != 5) { return; }

        if (UPGSO.upGradeID == 0) //life steal
        {
            lifeSteal *= UPGSO.upGradeAmount;
        }
        if (UPGSO.upGradeID == 1) //defence
        {
            defence *= UPGSO.upGradeAmount;
        }
        if (UPGSO.upGradeID == 2) //health
        {
            PlayerHealth.instance.increaseHealth(UPGSO.upGradeAmount);
        }
        if (UPGSO.upGradeID == 3) //xp damage
        {
            XPdropDamage = true;
        }
        if (UPGSO.upGradeID == 4) //xp multiplier
        {
            extraOrb += UPGSO.upGradeAmount;
        }
        if (UPGSO.upGradeID == 5) //xp multiplier
        {
            SpeedMultiplier *= UPGSO.upGradeAmount;
            playerMovement.instance.runSpeed *= SpeedMultiplier;
            playerMovement.instance.speedBuff += playerMovement.instance.runSpeed;
        }

    }
}
