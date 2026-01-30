using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class upGradeButton : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI description;
    public TextMeshProUGUI amountText;
    public UpGradeSO upGradeSO;

    public void Awake()
    {        
        upGradeSO = UpGradeManager.instance.getRandomSO();
        image.sprite = upGradeSO.Image;
        Name.text = upGradeSO.Name;
        description.text = upGradeSO.Description;
        if(upGradeSO.upGradeAmount == 0)
        {
            amountText.SetText("");
        }
        else
        {
            if (upGradeSO.plus)
            {
                amountText.SetText("+" + upGradeSO.upGradeAmount.ToString());
            }
            else
            {
                amountText.SetText("x" + upGradeSO.upGradeAmount.ToString());
            }
        }
        
    }

    public void upgrade()
    {
        UpGradeManager.instance.UpGrade(upGradeSO);
        scoreManager.instance.closeLevelUpMenu();
        Destroy(gameObject);
    }
}
