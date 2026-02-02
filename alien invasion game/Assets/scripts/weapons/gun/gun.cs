using EZCameraShake;
using SmallHedge.SoundManager;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gun : MonoBehaviour, Abilities
{
    //bool so that the ability interface actually works
    public bool unlocked { get; set; }


    /* ─────────────── WEAPON STATS ─────────────── */
    [Header("Weapon Stats")]
    public float damage;
    public float knockBack;
    public float range;
    public float fireRate;
    public float bulletsPerShot;
    public float spread;

    /* ─────────────── AMMO ─────────────── */
    [Header("Ammo")]
    public float ammo;
    [HideInInspector] public float bulletsLeft;
    public float reloadTime;
    private bool reloading;

    /* ─────────────── REFERENCES ─────────────── */
    [Header("References")]
    public Camera cam;
    public Transform[] gunTips;
    public Slider ammoSlider;
    public GameObject reloadIcon;

    /* ─────────────── VISUAL EFFECTS ─────────────── */
    [Header("Visual Effects")]
    public GameObject muzzleFlash;
    public GameObject impactEffect;
    public GameObject hitFlash;
    public float hitFlashDuration;

    /* ─────────────── CAMERA SHAKE ─────────────── */
    [Header("Camera Shake")]
    public float shootShakePower;
    public float shootShakeDuration;

    /* ─────────────── EXPLOSIONS ─────────────── */
    [Header("Explosion")]
    public explosionObj redExplosion;

    /* ─────────────── PRIVATE STATE ─────────────── */
    private float nextTimeToFire;
    private int currentGunTip;

    /* ─────────────── UNITY METHODS ─────────────── */
    private void Awake()
    {
        currentGunTip = 0;
        bulletsLeft = ammo;
        hitFlash.SetActive(false);
    }

    private void Update()
    {
        if (GetComponentInParent<PlayerHealth>().alive == false) return;

        UpdateAmmoUI();

        if (CanShoot())
        {
            Shoot();
        }

        if ((Input.GetKeyDown(KeyCode.R) || bulletsLeft <= 0) && !reloading)
        {
            StartReload();
        }
    }

    /* ─────────────── SHOOTING ─────────────── */
    private bool CanShoot()
    {
        return Input.GetMouseButton(0)
            && Time.time >= nextTimeToFire
            && bulletsLeft > 0
            && !reloading;
    }

    private void Shoot()
    {
        bulletsLeft--;
        if(bulletsLeft > ammo) { bulletsLeft = ammo; }
        nextTimeToFire = Time.time + 1f / fireRate;

        SoundManager.PlaySound(SoundType.shoot);
        CameraShaker.Instance.ShakeOnce(shootShakePower, 0.5f, 0, shootShakeDuration);

        PlayMuzzleFlash();
        CycleGunTip();

        for (int i = 0; i < bulletsPerShot; i++)
        {
            FireRay();
        }
    }

    private void FireRay()
    {
        Vector3 bulletSpread = cam.transform.forward +
            new Vector3(
                Random.Range(-spread, spread),
                Random.Range(-spread, spread),
                0f
            );

        if (Physics.Raycast(cam.transform.position, bulletSpread, out RaycastHit hit, range))
        {
            SpawnImpact(hit);

            if (hit.transform.TryGetComponent(out EnemyHealth enemy))
            {
                enemy.TakeDamage(damage);
                StartCoroutine(HitFlashRoutine());
            }

            if (hit.transform.TryGetComponent(out Rigidbody EnemyRB))
            {
                EnemyRB.AddForce(bulletSpread * knockBack, ForceMode.Impulse);
            }

            if (hit.transform.TryGetComponent(out bomb bomb))
            {
                CameraShaker.Instance.ShakeOnce(4, 15, 0, 3);
                bomb.explode(redExplosion);
            }
        }
    }

    /* ─────────────── RELOAD ─────────────── */
    private void StartReload()
    {
        reloading = true;
        Invoke(nameof(EndReload), reloadTime);
    }

    private void EndReload()
    {
        bulletsLeft = ammo;
        reloading = false;
    }

    /* ─────────────── EFFECTS ─────────────── */
    private void PlayMuzzleFlash()
    {
        GameObject flash = Instantiate(muzzleFlash, gunTips[currentGunTip].position, gunTips[currentGunTip].rotation);
        Destroy(flash, 0.5f);
    }

    private void SpawnImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 0.7f);
    }

    private IEnumerator HitFlashRoutine()
    {
        hitFlash.SetActive(true);
        yield return new WaitForSeconds(hitFlashDuration);
        hitFlash.SetActive(false);
    }

    /* ─────────────── UTILITIES ─────────────── */
    private void CycleGunTip()
    {
        currentGunTip++;
        if (currentGunTip >= gunTips.Length)
            currentGunTip = 0;
    }

    private void UpdateAmmoUI()
    {
        reloadIcon.SetActive(reloading);

        reloadIcon.transform.Rotate(0, 0, -10);

        float targetValue = bulletsLeft / ammo;
        ammoSlider.value = Mathf.Lerp(ammoSlider.value, targetValue, 0.25f);
    }

    /* ─────────────── Upgrade ─────────────── */

    public void upgrade(UpGradeSO UPGSO)
    {
        if (UPGSO.weaponID != 0) { return; }

        if(UPGSO.upGradeID == 0)
        {
            damage *= UPGSO.upGradeAmount;
        }
        if (UPGSO.upGradeID == 1)
        {
            fireRate *= UPGSO.upGradeAmount;
        }
        if (UPGSO.upGradeID == 2)
        {
            knockBack *= UPGSO.upGradeAmount;
        }
        if (UPGSO.upGradeID == 3)
        {
            ammo *= UPGSO.upGradeAmount;
            bulletsLeft *= UPGSO.upGradeAmount;
        }
        if (UPGSO.upGradeID == 4)
        {
            reloadTime *= UPGSO.upGradeAmount;
        }
    }
}
