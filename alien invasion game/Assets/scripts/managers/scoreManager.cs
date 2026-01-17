using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scoreManager : MonoBehaviour
{
    public static scoreManager instance;

    public int currentWave, currentKill;

    public TextMeshProUGUI killText, waveText;

    

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        killText.SetText("kills :" + currentKill.ToString());
        waveText.SetText("wave :" + currentWave.ToString());
    }

    public void UpdateKill()
    {
        currentKill++;
    }

    public void UpdateWave()
    {
        currentWave++;
    }
}
