using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new upgrade", menuName = "scriptable objects/upgrade")]
public class UpGradeSO : ScriptableObject
{
    [Header("UI")]
    public Sprite Image;
    public string Name;
    [Header("stats")]
    public int weaponID;
    public int upGradeID;
    public float upGradeAmount;

    //1 - gun
    //2 - bomb
    //3 chainsaw
}
