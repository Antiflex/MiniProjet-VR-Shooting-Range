using UnityEngine;
using System.Collections.Generic;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxTargets = 4;

    [Header("Zone de spawn")]
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 size;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0;
            TrySpawnTarget();
        }
    }

    void TrySpawnTarget()
    {
        int activeCount = CountActiveTargets();

        if (activeCount >= maxTargets)
            return;

        GameObject target = TargetPool.Instance.GetTarget();
        target.transform.position = GetRandomPoint();
        target.SetActive(true);
    }

    int CountActiveTargets()
    {
        int count = 0;

        foreach (var t in FindObjectsOfType<TargetBehaviour>())
        {
            if (t.gameObject.activeInHierarchy)
                count++;
        }

        return count;
    }

    Vector3 GetRandomPoint()
    {
        return center + new Vector3(
            Random.Range(-size.x / 2, size.x / 2),
            Random.Range(-size.y / 2, size.y / 2),
            Random.Range(-size.z / 2, size.z / 2)
        );
    }
}
