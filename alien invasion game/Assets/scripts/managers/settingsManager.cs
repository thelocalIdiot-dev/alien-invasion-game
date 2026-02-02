using DG.Tweening;
using EZCameraShake;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class settingsManager : MonoBehaviour
{
    public static settingsManager instance;

    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    [Header("Flags")]
    public bool mainMenu;

    [Header("Camera")]
    public CameraLook camLook;
    public CameraEffects camEffects;
    public Slider sensSlider;
    public Slider fovSlider;

    [Header("Audio UI")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Audio")]
    public AudioMixer mixer;

    [Header("Saved Values")]
    public float FOV;
    public float SENS;
    public float master;
    public float music;
    public float sound;

    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject);

        LoadSettings();
    }

    private void OnEnable()
    {
        // --- UI sync ONLY ---
        if (sensSlider) sensSlider.SetValueWithoutNotify(SENS);
        if (fovSlider) fovSlider.SetValueWithoutNotify(FOV);

        if (masterSlider) masterSlider.SetValueWithoutNotify(master);
        if (musicSlider) musicSlider.SetValueWithoutNotify(music);
        if (sfxSlider) sfxSlider.SetValueWithoutNotify(sound);
    }

    private void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if ((float)resolutions[i].refreshRateRatio.value == currentRefreshRate)
                filteredResolutions.Add(resolutions[i]);
        }

        filteredResolutions.Sort((a, b) =>
        {
            if (a.width != b.width)
                return b.width.CompareTo(a.width);
            return b.height.CompareTo(a.height);
        });

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            Resolution r = filteredResolutions[i];
            options.Add($"{r.width}x{r.height} {r.refreshRateRatio.value:0.##} Hz");

            if (r.width == Screen.width && r.height == Screen.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();

        ApplyAudio();
    }

    private void Update()
    {
        if (mainMenu) return;

        camLook.sensitivity = SENS;
        camEffects.baseFOV = FOV;
        camEffects.dashingFOV = FOV * 1.1111111111f;
        
    }

    // =========================
    // Resolution
    // =========================
    public void SetResolution()
    {
        Resolution resolution = filteredResolutions[currentResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
        SaveSettings();
    }

    // =========================
    // Save / Load
    // =========================
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("master", master);
        PlayerPrefs.SetFloat("music", music);
        PlayerPrefs.SetFloat("sound", sound);

        PlayerPrefs.SetInt("resolution", currentResolutionIndex);

        PlayerPrefs.SetFloat("sens", SENS);
        PlayerPrefs.SetFloat("fov", FOV);


        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        master = PlayerPrefs.GetFloat("master", 1f);
        music = PlayerPrefs.GetFloat("music", 1f);
        sound = PlayerPrefs.GetFloat("sound", 1f);

        currentResolutionIndex = PlayerPrefs.GetInt("resolution", 0);

        SENS = PlayerPrefs.GetFloat("sens", 50f);
        FOV = PlayerPrefs.GetFloat("fov", 90f);
    }

    // =========================
    // Camera
    // =========================
    public void SetFov(float level)
    {
        FOV = level;
        SaveSettings();
    }

    public void SetSensitivity(float level)
    {
        SENS = level;
        SaveSettings();
    }

    // =========================
    // Audio
    // =========================
    void ApplyAudio()
    {
        SetMasterVolume(master);
        SetMusicVolume(music);
        SetSfxVolume(sound);
    }

    public void SetMasterVolume(float level)
    {
        master = level;
        mixer.SetFloat("Master", LinearToDb(level));
        SaveSettings();
    }

    public void SetMusicVolume(float level)
    {
        music = level;
        mixer.SetFloat("Music", LinearToDb(level));
        SaveSettings();
    }

    public void SetSfxVolume(float level)
    {
        sound = level;
        mixer.SetFloat("sfx", LinearToDb(level));
        SaveSettings();
    }

    float LinearToDb(float value)
    {
        print(Mathf.Log10(value) * 20);
        return Mathf.Log10(value) * 20;
    }

}
