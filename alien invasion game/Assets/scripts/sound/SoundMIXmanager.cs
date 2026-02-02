using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Timeline;

public class SoundMIXmanager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public settingsManager settingsManager;
    public static SoundMIXmanager instance;

    private void Awake()
    {
        instance = this;
      
    }

    
}
