using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class upGradeButton : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI description;
    public TextMeshProUGUI amountText;
    UpGradeSO upGradeSO;

    public void Awake()
    {
        Debug.Log("<qlefiu h");
        upGradeSO = UpGradeManager.instance.upgrades[Random.Range(0, UpGradeManager.instance.upgrades.Length)];
        image.sprite = upGradeSO.Image;
        description.text = upGradeSO.Name;
        amountText.SetText("x" + upGradeSO.upGradeAmount.ToString());
    }

    public void upgrade()
    {
        UpGradeManager.instance.UpGrade(upGradeSO);
        scoreManager.instance.closeLevelUpMenu();
        Destroy(gameObject);
    }
}
