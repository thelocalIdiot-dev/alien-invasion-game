using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public Animator transition;
    public float waitTime;

    public static SceneLoader instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void loadScene(int sceneIndex)
    {
        StartCoroutine(loadSceneWithTransition(sceneIndex));
        Debug.Log("Loading scene: " + sceneIndex);
    }

    IEnumerator loadSceneWithTransition(int sceneIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(sceneIndex);

    }
}

