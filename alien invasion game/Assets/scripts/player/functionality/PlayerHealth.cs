using EZCameraShake;
using SmallHedge.SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour , Damageable
{
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float duration, screenFlashDuration;
    [SerializeField] private Color screenFlashColor;

    public SkinnedMeshRenderer meshRenderer;
    Material originalMaterial;
    private Coroutine flashRoutine;
    public UnityEngine.UI.Slider healthSlider;
    public GameObject deathPartical;
    public GameObject hudCanvas;
    public GameObject DeathCanvas;
    public GameObject enemies;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI tipText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI highScoreText;

    public string[] Tips;

    public Volume healthVolume;

    public float currentHealth, MaxHealth = 100;

    public bool alive;

    public bool invinsible;

    public static PlayerHealth instance;
    void Awake()
    {
        instance = this;
        currentHealth = MaxHealth;
        originalMaterial = meshRenderer.material;
        DeathCanvas.SetActive(false);
        alive = true;
    }

    public void TakeDamage(float amount)
    {
        if(invinsible || !alive) { return; }
        currentHealth -= amount / valueManager.instance.defence;
        Flash();
        CameraShaker.Instance.ShakeOnce(2, 10f, 0, .2f);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void increaseHealth(float amount)
    {
        MaxHealth += amount;
        currentHealth += amount;
    }

    private void Update()
    {
        if (currentHealth > MaxHealth)
        {
            currentHealth = MaxHealth;
        }

        healthSlider.maxValue = MaxHealth;
        healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, 0.25f);
        float desiredText = Mathf.Round(healthSlider.value);
        healthText.SetText(desiredText.ToString());



        healthVolume.weight = Mathf.Abs(1-(currentHealth / MaxHealth));
    }

    void Die()
    {
        hudCanvas.SetActive(false);
        GetComponent<playerMovement>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponentInChildren<Animator>().SetFloat("speed", 0);
        alive = false;
        ScoreText.SetText("score:"+scoreManager.instance.currentWave.ToString());
        highScoreText.SetText("high score:" + PlayerPrefs.GetInt("high score", scoreManager.instance.currentWave));
        if (PlayerPrefs.GetInt("high score", 1) < scoreManager.instance.currentWave)
        {
            PlayerPrefs.SetInt("high score", scoreManager.instance.currentWave);
        }        
        Invoke(nameof(explode), 1);
    }

    void explode()
    {
        SoundManager.PlaySound(SoundType.playerDeath);
        Instantiate(deathPartical, transform.position, Quaternion.identity);
        CameraShaker.Instance.ShakeOnce(10, 20f, 0, .9f);
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        tipText.SetText("Tip:"+Tips[UnityEngine.Random.Range(0, Tips.Length)]);
        DeathCanvas.SetActive(true);
        meshRenderer.enabled = false;
    }

    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {       
        meshRenderer.material = flashMaterial;

        screenFlash.instance.ScreenFlash(screenFlashColor, screenFlashDuration);

        yield return new WaitForSeconds(duration);

        meshRenderer.material = originalMaterial;

        flashRoutine = null;
    }
}
