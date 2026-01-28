using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public enum GS
    //{
    //    mainMenu,
    //    playing,
    //    paused,
    //    gameOver
    //}
    //
    //public GS GameState;

    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void GoToNextLevel()
    {
        SceneLoader.instance.loadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }  
    public void restart()
    {
       
        StartCoroutine(restartLevel());
    }
    IEnumerator restartLevel()
    {
        yield return new WaitForSeconds(0.6f);

        SceneLoader.instance.loadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneLoader.instance.loadScene(0);
    }
}
