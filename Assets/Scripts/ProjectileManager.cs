using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance;

    [Header("Pool")]
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] private int initialPoolSize = 20;

    [Header("Cleanup")]
    [SerializeField] private float maxDistance = 40f;
    [SerializeField] private float checkInterval = 0.3f;

    private Queue<GameObject> pool = new Queue<GameObject>();
    private List<GameObject> activeProjectiles = new List<GameObject>();

    private Transform origin;

    private void Awake()
    {
        Instance = this;
        origin = transform;

        for (int i = 0; i < initialPoolSize; i++)
            CreateBullet();

        StartCoroutine(CleanupLoop());
    }

    private GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);
        pool.Enqueue(bullet);
        return bullet;
    }

    // ================================
    // UTILISÉ PAR LE TIR
    // ================================
    public GameObject GetBullet()
    {
        if (pool.Count == 0)
            CreateBullet();

        GameObject bullet = pool.Dequeue();
        bullet.SetActive(true);

        activeProjectiles.Add(bullet);
        return bullet;
    }

    // ================================
    // RETOUR AU POOL
    // ================================
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        activeProjectiles.Remove(bullet);
        pool.Enqueue(bullet);
    }

    // ================================
    // COROUTINE DE NETTOYAGE
    // ================================
    private IEnumerator CleanupLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            for (int i = activeProjectiles.Count - 1; i >= 0; i--)
            {
                GameObject bullet = activeProjectiles[i];

                if (!bullet.activeInHierarchy)
                {
                    activeProjectiles.RemoveAt(i);
                    continue;
                }

                float distance = Vector3.Distance(origin.position, bullet.transform.position);

                if (distance > maxDistance)
                {
                    ReturnBullet(bullet);
                }
            }
        }
    }
}
