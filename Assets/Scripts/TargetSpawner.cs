using System.Collections;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 2f;

    [Header("Zone de spawn")]
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 size;

    private void OnEnable()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (GameplayManager.Instance == null) continue;
            if (!GameplayManager.Instance.CanSpawnTarget()) continue;

            GameObject t = TargetPool.Instance.GetTarget();
            t.transform.position = GetRandomPoint();
            t.SetActive(true);

            GameplayManager.Instance.RegisterTargetSpawn();
        }
    }

    private Vector3 GetRandomPoint()
    {
        return center + new Vector3(
            Random.Range(-size.x / 2f, size.x / 2f),
            Random.Range(-size.y / 2f, size.y / 2f),
            Random.Range(-size.z / 2f, size.z / 2f)
        );
    }
}
