using UnityEngine;
using UnityEngine.AddressableAssets;

public class FXAddressables : MonoBehaviour
{
    public static FXAddressables Instance;
    private string platform;

    // ---- Keys Addressables ----
    [Header("PCVR Keys")]
    [SerializeField] private string muzzlePC_Key;
    [SerializeField] private string explosionPC_Key;
    [SerializeField] private string targetPC_Key;

    [Header("Meta Quest Keys")]
    [SerializeField] private string muzzleMQ_Key;
    [SerializeField] private string explosionMQ_Key;
    [SerializeField] private string targetMQ_Key;

    // ---- Prefabs chargés dynamiquement ----
    public GameObject muzzleFX { get; private set; }
    public GameObject explosionFX { get; private set; }
    public GameObject targetPrefab { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
#if UNITY_ANDROID
        platform = "Meta Quest";
        LoadPlatformAssets(muzzleMQ_Key, explosionMQ_Key, targetMQ_Key);
#else
        platform = "PC VR";
        LoadPlatformAssets(muzzlePC_Key, explosionPC_Key, targetPC_Key);
#endif
    }

    private void LoadPlatformAssets(string muzzleKey, string explosionKey, string targetKey)
    {
        Debug.Log($"[FXAddressables] Loading assets for platform: {platform}");
        // Load Muzzle FX
        Addressables.LoadAssetAsync<GameObject>(muzzleKey).Completed += handle =>
        {
            muzzleFX = handle.Result;
        };

        // Load Explosion FX
        Addressables.LoadAssetAsync<GameObject>(explosionKey).Completed += handle =>
        {
            explosionFX = handle.Result;
        };

        // Load Target Prefab (nécessaire pour le pool)
        Addressables.LoadAssetAsync<GameObject>(targetKey).Completed += handle =>
        {
            targetPrefab = handle.Result;

            // On injecte ce prefab dans le TargetPool
            if (TargetPool.Instance != null)
                TargetPool.Instance.InitializeAddressablePrefab(targetPrefab);
        };
    }
}
