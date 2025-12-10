using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [SerializeField] private GameObject bulletPrefab;   // Prefab classique dans le projet (non-Addressable)
    [SerializeField] private int poolSize = 20;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
        PopulatePool();
    }

    private void PopulatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetBullet()
    {
        if (pool.Count == 0)
        {
            // Sécurité : si toutes les balles sont utilisées, on en recrée une
            GameObject extra = Instantiate(bulletPrefab);
            extra.SetActive(false);
            pool.Enqueue(extra);
        }

        GameObject bullet = pool.Dequeue();
        bullet.SetActive(true);
        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);
    }
}
