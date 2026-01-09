using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float _explosionRadius = 5;
    [SerializeField] private float _explosionForce = 700;
    [SerializeField] private GameObject _particles;
    //public LayerMask IgnoreLayer;
    private void OnTriggerEnter(Collider other)
    {
        explode();
    }

    public void explode()
    {
        var surroundingObjects = Physics.OverlapSphere(transform.position, _explosionRadius);

        SoundManager.PlaySound(SoundType.explosion);

        foreach (var obj in surroundingObjects)
        {
            var rb = obj.GetComponent<Rigidbody>();
            var Healths = obj.GetComponent<Damageable>();
            if (Healths != null) { Healths.TakeDamage(damage); }
            //float distance = Vector3.Distance(obj.transform.position, transform.position);
            //float falloff = Mathf.Clamp01(1 - (distance / _explosionRadius));
            //
            //if (rb == null) continue;
            //rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 1);
        }

        GameObject explosionObj = Instantiate(_particles, transform.position, Quaternion.identity);

        Destroy(explosionObj, 1);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, _explosionRadius);
    }
}
