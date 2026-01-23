using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public bool Froze;

    private void Awake()
    {
        instance = this;
    }

    public void Freeze(bool unlockCursor)
    {
        Time.timeScale = 0;
        if(unlockCursor)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        
        Froze = true;
    }

    public void UnFreeze()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;

        Froze= false;
    }
}
