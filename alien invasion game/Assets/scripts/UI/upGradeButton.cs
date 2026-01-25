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
    public UpGradeSO upGradeSO;

    public void Awake()
    {        
        upGradeSO = UpGradeManager.instance.getRandomSO();
        image.sprite = upGradeSO.Image;
        description.text = upGradeSO.Name;
        amountText.SetText("x" + upGradeSO.upGradeAmount.ToString());
    }

    public void upgrade()
    {
        UpGradeManager.instance.UpGrade(upGradeSO);
        scoreManager.instance.closeLevelUpMenu();
        Debug.Log("BUTTON WORKS BOYYY");
        Destroy(gameObject);
    }
}
