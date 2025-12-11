using System.Collections.Generic;
using UnityEngine;

public class TargetPool : MonoBehaviour
{
    public static TargetPool Instance;

    private GameObject targetPrefab; // fourni par FXAddressables
    private List<GameObject> pool = new List<GameObject>();

    [SerializeField] private int initialPoolSize = 10;

    private void Awake()
    {
        Instance = this;
    }

    public void InitializeAddressablePrefab(GameObject prefab)
    {
        targetPrefab = prefab;

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

        return CreateNewTarget();
    }
}
