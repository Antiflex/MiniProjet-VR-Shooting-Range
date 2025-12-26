using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    [Header("Score")]
    [SerializeField] private int currentScore = 0;

    [Header("Targets")]
    [SerializeField] private int maxTargets = 4;
    private int currentTargets = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void ResetGame()
    {
        currentScore = 0;
        currentTargets = 0;
    }

    // ---- Score ----
    public void AddScore(int amount) => currentScore += amount;
    public int GetScore() => currentScore;

    // ---- Targets ----
    public int GetMaxTargets() => maxTargets;
    public int GetCurrentTargets() => currentTargets;

    public bool CanSpawnTarget() => currentTargets < maxTargets;

    public void RegisterTargetSpawn() => currentTargets++;
    public void RegisterTargetDespawn() => currentTargets = Mathf.Max(0, currentTargets - 1);
}
