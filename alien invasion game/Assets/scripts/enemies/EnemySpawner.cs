using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Vector3 boxSize;

    public Transform enemyParent;

    public float spawnDelay, spawnTimer; 
    
    public int baseSwarmSize;

    //public float swarmSizeMultiplier = 1.2f, multiplieDelay, multiplieTimer;

    public enemies[] enemyList;

    [Header("NavMesh")]
    [SerializeField] private float navMeshSampleRadius = 2f;
    [SerializeField] private int maxSpawnAttempts = 10;

    void Update()
    {
        if (enemyParent.childCount == 0)
        {
            if (spawnTimer > spawnDelay)
            {
                SpawnSwarm();
                spawnTimer = 0;
            }
            else
            {
                spawnTimer += Time.deltaTime;
            }
        }
    }

    int GetRandomValue()
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
                return i;
        }

        return enemyList.Length - 1;
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

    void SpawnSwarm()
    {
        scoreManager.instance.UpdateWave();
        SoundManager.PlaySound(SoundType.EnemySpawn);

        int finalSwarmSize = Random.Range(1, baseSwarmSize) * scoreManager.instance.currentWave + 1;

        for (int i = 0; i < finalSwarmSize; i++)
        {
           
            Spawn();
        }
    }

    public void Spawn()
    {
        int prefabId = GetRandomValue();

        // Check if this enemy uses NavMesh
        bool IsGrounded = enemyList[prefabId].grounded;

        Vector3 spawnPos = GetRandomPoint();

        if (IsGrounded)
        {
            if (!TryGetNavMeshPoint(out spawnPos))
            {
                Debug.LogWarning($"Failed to find NavMesh spawn point for {enemyList[prefabId].mob.name}");
                return;
            }
            //else
            //{
            //    SoundManager.PlaySound(SoundType.EnemySpawn);
            //}
        }
        else
        {
            //SoundManager.PlaySound(SoundType.EnemySpawn);
        }

        GameObject EP = Instantiate(enemyList[prefabId].mob, spawnPos, Quaternion.identity);
        EP.transform.parent = enemyParent;
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
        public bool grounded;
        public float probability;
    }
}
