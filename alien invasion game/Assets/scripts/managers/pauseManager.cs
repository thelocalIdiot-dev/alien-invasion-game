using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseManager : MonoBehaviour
{
    public bool pause;

    public GameObject PauseMenu;

    public static pauseManager instance;

    private void Awake()
    {
        instance = this; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pause == false)
        {
            Pause();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && pause == true)
        {
            UnPause();
        }

        PauseMenu.SetActive(pause);

    }

    private void Pause()
    {
        Time.timeScale = 0;
        pause = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void UnPause()
    {
        if (!TimeManager.instance.Froze)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }       
        pause = false;
    }
}
