using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Vector3 boxSize;

    public float spawnDelay, spawnTimer;
    public float spawnDelayMultiplier = 0.9f, multiplieDelay, multiplieTimer;

    public enemies[] enemyList;

    [Header("NavMesh")]
    [SerializeField] private float navMeshSampleRadius = 2f;
    [SerializeField] private int maxSpawnAttempts = 10;

    void Update()
    {
        if (spawnTimer > spawnDelay)
        {
            Spawn();
            spawnTimer = 0;
        }
        else
        {
            spawnTimer += Time.deltaTime;
        }

        if (multiplieTimer > multiplieDelay)
        {
            spawnDelay *= spawnDelayMultiplier;
            multiplieTimer = 0;
        }
        else
        {
            multiplieTimer += Time.deltaTime;
        }
    }

    GameObject GetRandomValue()
    {
        float totalWeight = 0f;

        for (int i = 0; i < enemyList.Length; i++)
            totalWeight += enemyList[i].probability;

        float randomPoint = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        for (int i = 0; i < enemyList.Length; i++)
        {
            currentWeight += enemyList[i].probability;
            if (randomPoint <= currentWeight)
                return enemyList[i].mob;
        }

        return enemyList[enemyList.Length - 1].mob;
    }

    Vector3 GetRandomPoint()
    {
        Vector3 half = boxSize * 0.5f;

        return transform.position + new Vector3(
            Random.Range(-half.x, half.x),
            Random.Range(-half.y, half.y),
            Random.Range(-half.z, half.z)
        );
    }

    public void Spawn()
    {
        GameObject prefab = GetRandomValue();

        // Check if this enemy uses NavMesh
        bool usesNavMesh = prefab.GetComponent<NavMeshAgent>() != null;

        Vector3 spawnPos = GetRandomPoint();

        SoundManager.PlaySound(SoundType.EnemySpawn);      

        if (usesNavMesh)
        {
            if (!TryGetNavMeshPoint(out spawnPos))
            {
                Debug.LogWarning($"Failed to find NavMesh spawn point for {prefab.name}");
                return;
            }
        }

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    bool TryGetNavMeshPoint(out Vector3 result)
    {
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            Vector3 randomPoint = GetRandomPoint();

            if (NavMesh.SamplePosition(
                randomPoint,
                out NavMeshHit hit,
                navMeshSampleRadius,
                NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }

    [System.Serializable]
    public struct enemies
    {
        public GameObject mob;
        public float probability;
    }
}
