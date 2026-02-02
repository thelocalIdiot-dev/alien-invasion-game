using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectOrb : MonoBehaviour
{
    public float duration;
    public GameObject particals;    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {          
            playerMovement.instance.SpeedEffect(duration);
            screenFlash.instance.ScreenFlash(Color.cyan, 0.4f);

            GameObject particalObject = Instantiate(particals, transform.position, Quaternion.identity);
            Destroy(particalObject, 0.6f);
            Destroy(gameObject);
        }
    }

}
