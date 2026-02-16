using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagePopupManager : MonoBehaviour
{
    public GameObject damagePopup;
    public float lifeTime;
    public float midThreshold, highThreshold;
    public Color baseColor = Color.white, midColor = Color.yellow, highColor = Color.red;

    public static damagePopupManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void spawnPopup(int damage, Vector3 position)
    {
        GameObject DP = Instantiate(damagePopup, position, Quaternion.identity);
        TextMesh text = DP.GetComponent<TextMesh>();
        text.text = damage.ToString();
        if(damage >= highThreshold)
        {
            text.color = highColor;
        }
        else if (damage >= midThreshold)
        {
            text.color = midColor;
        }
        else 
        {
            text.color = baseColor;
        }
        
        Destroy(DP, lifeTime);
    }
}
