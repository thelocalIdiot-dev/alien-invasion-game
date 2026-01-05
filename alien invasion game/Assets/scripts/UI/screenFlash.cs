using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class screenFlash : MonoBehaviour
{
    Image image;


    public static screenFlash instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void ScreenFlash(Color color, float duration)
    {
        
        //StopAllCoroutines();
        StartCoroutine(FlashRoutine(color, duration));
    }

    private IEnumerator FlashRoutine(Color c, float d)
    {
        image.color = c; // Instantly turn white
        float elapsedTime = 0f;

        while (elapsedTime < d)
        {
            elapsedTime += Time.deltaTime;
            image.color = Color.Lerp(c, Color.clear, elapsedTime / d);
            yield return null;
        }

        image.color = Color.clear;

        
        //
        //image.color = c;
        //
        //yield return new WaitForSeconds(d);
        //
        //image.color = Color.clear;
    }
}
