using EZCameraShake;
using SmallHedge.SoundManager;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gun : MonoBehaviour
{

    public float dammage;

    public float range;

    public Camera cam;

    public float fireRate = 15f;

    public Transform[] GunTips;

    public GameObject MuzzelFlash;

    public GameObject ImpactEffect;

    private float nextTimeToFire;

    public GameObject hitFlash;

    public float hitFlashDuration;

    public float ShakePower;

    public float num_bullets = 1;

    public float spread;

    Vector3 HitPoint;

    public float ammo;

    [HideInInspector] public float bulletsLeft;

    bool reloading;

    public float reloadTime;

    public Slider bullets;

    int currentGP;

    public explosionObj redExploion;
    private void Start()
    {
        hitFlash.SetActive(false);
        bulletsLeft = ammo;
    }

    void Awake()
    {
        currentGP = 0;
    }

    void Update()
    {
        float desiredValue = bulletsLeft / ammo;
        bullets.value = Mathf.Lerp(bullets.value, desiredValue, 0.25f);

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire && bulletsLeft > 0 && ! reloading)
        {
            bulletsLeft--;

            SoundManager.PlaySound(SoundType.shoot);
            CameraShaker.Instance.ShakeOnce(1, 0.5f, 0, .1f);

            nextTimeToFire = Time.time + 1/fireRate;

            currentGP++;
            if(currentGP > GunTips.Length - 1)
            {
                currentGP = 0;
            }

            effects(currentGP);

            for (int i = 0; i < num_bullets; i++)
            {
                shoot();
            }                      
        }
       
            
        if((Input.GetKeyDown(KeyCode.R) || bulletsLeft == 0) && !reloading)
        {
            Reload();
        }
    }

    private void Reload()
    {
        reloading = true;
      
        Invoke(nameof(endReload), reloadTime);
    }

    void endReload()
    {
        bulletsLeft = ammo;
        reloading = false;
    }

    void effects(int currentGunTip)
    {
        GameObject MGO = Instantiate(MuzzelFlash, GunTips[currentGunTip].position, GunTips[currentGunTip].rotation);
        Destroy(MGO, 0.5f);

    }

    private void shoot()
    {
        float Spreadx = Random.Range(-spread, spread);
        float Spready = Random.Range(-spread, spread);

        Vector3 BulletSpread = new Vector3(Spreadx, Spready, 0);
      
        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward + BulletSpread, out hit))
        {
            GameObject Impact = Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(Impact, 0.7f);

            

            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
          
            bomb Bomb = hit.transform.GetComponent<bomb>();
          
            if (target != null)
            {
                target.TakeDamage(dammage);
          
                StartCoroutine(DOhitFlash());
            }
          
            if (Bomb != null)
            {
                CameraShaker.Instance.ShakeOnce(4, 15, 0, 3);
                Bomb.explode(redExploion);
            }
        }
    }

    private IEnumerator DOhitFlash()
    {
        hitFlash.SetActive(true);

        yield return new WaitForSeconds(hitFlashDuration);

        hitFlash.SetActive(false);
    }


}
