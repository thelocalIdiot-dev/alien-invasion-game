using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
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

    public float currentHealth, MaxHealth = 100;

    public bool invinsible;
    void Awake()
    {
        currentHealth = MaxHealth;
        originalMaterial = meshRenderer.material;
    }

    public void TakeDamage(float amount)
    {
        if(invinsible) { return; }
        currentHealth -= amount;
        Flash();
        CameraShaker.Instance.ShakeOnce(2, 10f, 0, .2f);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Update()
    {
        if(currentHealth > MaxHealth)
        {
            currentHealth = MaxHealth;
        }

        healthSlider.maxValue = MaxHealth;
        healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, 0.25f);
    }

    void Die()
    {
        Destroy(gameObject);
    }


    [ContextMenu("flash")]
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
