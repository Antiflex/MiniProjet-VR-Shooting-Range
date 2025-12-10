using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("Prefab de la cible")]
    [SerializeField] private GameObject targetPrefab;

    [Header("Paramètres de spawn")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxTargets = 4;

    [Header("Zone de spawn")]
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 size;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            TrySpawnTarget();
        }
    }

    private void TrySpawnTarget()
    {
        int count = CountTargetsInScene();

        if (count >= maxTargets)
            return;

        Vector3 pos = GetRandomPoint();
        Instantiate(targetPrefab, pos, Quaternion.Euler(new Vector3(-90,0,0)));
    }

    private int CountTargetsInScene()
    {
        return FindObjectsOfType<TargetBehaviour>().Length;
    }

    private Vector3 GetRandomPoint()
    {
        return center + new Vector3(
            Random.Range(-size.x / 2, size.x / 2),
            Random.Range(-size.y / 2, size.y / 2),
            Random.Range(-size.z / 2, size.z / 2)
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, size);
    }
}
