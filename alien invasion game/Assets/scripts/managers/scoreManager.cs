using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scoreManager : MonoBehaviour
{
    public static scoreManager instance;

    public int currentWave, currentKill, currentLevel = 0;

    public float currentXP, XPrequirement, XpRequirementMultiplier;

    public TextMeshProUGUI killText, waveText;

    public Slider XpSlider;

    public GameObject partical;
    public GameObject levelUpScreen;

    private void Awake()
    {
        instance = this;
        levelUpScreen.SetActive(false);
    }

    private void Update()
    {
        killText.SetText("kills :" + currentKill.ToString());
        waveText.SetText("wave :" + currentWave.ToString());

        XpSlider.maxValue = XPrequirement;
        XpSlider.value = Mathf.Lerp(XpSlider.value, currentXP, 0.25f);

        if(currentXP > XPrequirement)
        {
            openLevelUpMenu();
        }        
    }

    public void openLevelUpMenu()
    {
        if(!PlayerHealth.instance.alive) { return; }
        levelUpScreen.SetActive(true);
        UpGradeManager.instance.setUpButton();
        TimeManager.instance.Freeze(true);
        currentLevel++;
        currentXP = 0;
        XPrequirement *= XpRequirementMultiplier;
    }

    public void closeLevelUpMenu()
    {
        GameObject obj = Instantiate(partical, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.LookRotation(new Vector3(0, 90, 0)));
        Destroy(obj, 3);
        
        levelUpScreen.SetActive(false);
        TimeManager.instance.UnFreeze();
    }

    public void UpdateKill()
    {
        currentKill++;
    }

    public void UpdateWave()
    {
        currentWave++;
    }

    public void UpdateXp(float amount)
    {
        currentXP += amount;
    }
}
