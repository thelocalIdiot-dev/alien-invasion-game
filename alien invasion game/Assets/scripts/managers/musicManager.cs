using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicManager : MonoBehaviour
{
    AudioSource musicSource;

    void Start()
    {
        musicSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHealth ph = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        musicSource.mute = !ph.alive;
    }
}
