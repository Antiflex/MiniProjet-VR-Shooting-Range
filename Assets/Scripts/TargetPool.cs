using System.Collections.Generic;
using UnityEngine;

public class TargetPool : MonoBehaviour
{
    public static TargetPool Instance;

    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private int initialPoolSize = 10;

    private List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < initialPoolSize; i++)
            CreateNewTarget();
    }

    private GameObject CreateNewTarget()
    {
        GameObject t = Instantiate(targetPrefab);
        t.SetActive(false);
        pool.Add(t);
        return t;
    }

    public GameObject GetTarget()
    {
        foreach (var t in pool)
        {
            if (!t.activeInHierarchy)
                return t;
        }

        // pool épuisé → on peut créer si tu veux que ça scale
        return CreateNewTarget();
    }
}
