using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new explosion", menuName = "scriptable objects/explosion")]
public class explosionObj : ScriptableObject
{
    // Start is called before the first frame update
    [Header("explosion")]
    public float damage;
    public float radius; 
    [Header("visuals")]
    public GameObject partical;  
    public float mag, rough, fadeOut; 
}
