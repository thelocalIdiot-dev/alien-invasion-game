using SmallHedge.SoundManager;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, Damageable
{
    [Header("Flash")]
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration = 0.1f;

    [Header("drops")]
    public GameObject healOrb;
    public float XP;
    [Range(0, 100)] public float propability;

    [Header("Health")]
    public float maxHealth = 300f;
    public float currentHealth;

    [Header("VFX")]
    public GameObject blood;
    public Transform bloodPosition;    


    private Renderer[] renderers;
    private Material[][] originalMaterials;
    private Coroutine flashRoutine;

    void Awake()
    {
        currentHealth = maxHealth;

        // Get ALL renderers (MeshRenderer + SkinnedMeshRenderer)
        renderers = GetComponentsInChildren<Renderer>();

        // Cache original materials
        originalMaterials = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Flash();
        SoundManager.PlaySound(SoundType.enemyHurt);
        if (currentHealth <= 0)
        {           
            Die();
        }
    }

    void Die()
    {
        if (bloodPosition != null)
        {
            GameObject gib = Instantiate(blood, bloodPosition.position, Quaternion.identity);
            Destroy(gib, 1f);
        }
        else if(bloodPosition == null)
        {
            GameObject gib = Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(gib, 1f);
        }
        scoreManager.instance.UpdateKill();
        scoreManager.instance.UpdateXp(XP);
        droploot(healOrb);
        Destroy(gameObject);
    }

    void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        // Apply flash material to all renderers
        foreach (Renderer r in renderers)
        {
            Material[] flashMats = new Material[r.materials.Length];
            for (int i = 0; i < flashMats.Length; i++)
                flashMats[i] = flashMaterial;

            r.materials = flashMats;
        }

        yield return new WaitForSeconds(flashDuration);

        // Restore original materials
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = originalMaterials[i];
        }

        flashRoutine = null;
    }

    void droploot(GameObject lootDrop)
    {
        float digit = Random.Range(1, 101);

        if (digit <= propability)
        {
            Instantiate(lootDrop, transform.position, Quaternion.identity);
        }       
    }

}
