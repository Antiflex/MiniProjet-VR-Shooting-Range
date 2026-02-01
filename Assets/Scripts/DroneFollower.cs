using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DroneFollower_Debug : MonoBehaviour
{
    [Header("Target")]
    public Transform followAnchor;

    [Header("Sampling")]
    [Range(1, 16)]
    public int rings = 4;                 // résolution verticale
    [Range(4, 32)]
    public int directionsPerRing = 12;    // résolution horizontale
    public float radialFalloff = 2.0f; // >0, plus grand = plus dense au centre
    [Range(5f, 120f)]
    public float coneAngle = 60f;
    public bool jitterSampling = true;




    [Header("Movement")]
    public float baseMoveSpeed = 1.0f;
    private float moveSpeed = 1.0f;
    public float directionSmoothing = 4.0f;
    public float hysteresisThreshold = 0.15f;

    [Header("Obstacle Avoidance")]
    public float lookAheadBase = 8.0f;
    public float lookAheadSpeedFactor = 1.0f;
    public float clearanceRadius = 0.3f;
    public LayerMask obstacleMask;

    [Header("Scoring Weights")]
    public float wFollow = 1.0f;
    public float wLoS = 2.0f;
    public float wTurn = 0.5f;
    public float wSafe = 1.0f;

    [Header("Fallback")]
    public float fallbackPauseTime = 0.5f;

    [Header("Debug")]
    public bool verboseLogs = true;
    public float debugRayLength = 2.0f;
    public float[] Scores;
    public float maxScore = 10f;

    // Internal
    private List<Vector3> candidateDirections = new();
    private List<int> feasibleIndices = new();

    private Vector3 bestDirection = Vector3.zero;
    private Vector3 currentDirection = Vector3.zero;

    private float fallbackTimer = 0f;

    private void Start()
    {
        moveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        if (!followAnchor)
        {
            Debug.LogWarning("[Drone] ❌ followAnchor NULL");
            return;
        }

        if (moveSpeed <= 0f)
        {
            Debug.LogWarning("[Drone] ❌ moveSpeed <= 0");
        }

        if (Vector3.Distance(transform.position, followAnchor.position) < clearanceRadius)
        {
            if (verboseLogs)
                Debug.Log("[Drone] ✅ Arrivé à la cible");
            return;
        }

        GenerateCandidateDirections();
        FilterObstacleCollisions();

        if (feasibleIndices.Count == 0)
        {
            if (verboseLogs)
                Debug.Log("[Drone] ⚠ Aucune direction faisable → fallback");
            HandleFallback(true);
            return;
        }

        HandleFallback(false); // Reset fallback if we have feasible directions
        ScoreDirections();
        ApplyMovement();
    }

    // ---------------- B1 ----------------
    // ---------------- B1 ----------------
    void GenerateCandidateDirections()
    {
        candidateDirections.Clear();

        Vector3 dronePos = transform.position;
        Vector3 toTarget = followAnchor.position - dronePos;

        // --- FIX ICI : On ajoute REELLEMENT le forward ---
        candidateDirections.Add(transform.forward);

        // On vérifie la distance pour éviter les erreurs de calcul si on est sur la cible
        if (toTarget.sqrMagnitude < 0.001f) toTarget = transform.forward;

        Vector3 d0 = transform.forward;

        // ... (Ton code de base orthonormée : right, up) ...
        Vector3 right = Vector3.Cross(d0, Vector3.up);
        if (right.sqrMagnitude < 0.001f) right = Vector3.Cross(d0, Vector3.forward);
        right.Normalize();
        Vector3 up = Vector3.Cross(right, d0);

        float coneRad = coneAngle * Mathf.Deg2Rad;

        // --- OPTIMISATION : On initialise Scores à la fin pour être sûr de la taille ---
        for (int ring = 0; ring < rings; ring++)
        {
            float r = (ring + 1f) / rings;
            float rWeighted = r / (1f + radialFalloff * r * r);
            float elevation = rWeighted * coneRad;
            float sinE = Mathf.Sin(elevation);
            float cosE = Mathf.Cos(elevation);

            for (int i = 0; i < directionsPerRing; i++)
            {
                float azimuth = (i / (float)directionsPerRing) * Mathf.PI * 2f;
                if (jitterSampling) azimuth += Random.Range(-0.5f, 0.5f) * (Mathf.PI * 2f / directionsPerRing);

                Vector3 dir = cosE * d0 + sinE * (Mathf.Cos(azimuth) * right + Mathf.Sin(azimuth) * up);
                candidateDirections.Add(dir.normalized);
            }
        }

        // On initialise le tableau une fois que tout est ajouté
        Scores = new float[candidateDirections.Count];
    }


    // ---------------- B2 ----------------
    void FilterObstacleCollisions()
    {
        feasibleIndices.Clear();

        Vector3 pos = transform.position;
        float lookAhead = lookAheadBase + lookAheadSpeedFactor * moveSpeed;

        for (int i = 0; i < candidateDirections.Count; i++)
        {
            Vector3 dir = candidateDirections[i];

            bool hit = Physics.SphereCast(
                pos,
                clearanceRadius,
                dir,
                out _,
                lookAhead,
                obstacleMask
            );

            if (!hit)
                feasibleIndices.Add(i);
        }

        if (verboseLogs)
            Debug.Log($"[Drone] B2: {feasibleIndices.Count}/{candidateDirections.Count} directions faisables");
    }


    // ---------------- B3 ----------------
    void ScoreDirections()
    {
        float bestScore = float.PositiveInfinity;
        bestDirection = Vector3.zero;

        Vector3 pos = transform.position;
        Vector3 targetPos = followAnchor.position;
        Vector3 dirToTarget = (targetPos - pos).normalized; // Vecteur directionnel vers cible

        for (int i = 0; i < Scores.Length; i++)
            Scores[i] = float.PositiveInfinity;

        foreach (int idx in feasibleIndices)
        {
            Vector3 dir = candidateDirections[idx];
            float score = 0f;

            // 1️⃣ Follow (Alignement avec la cible) 
            // 0 = aligné, 2 = opposé. On veut minimiser le score.
            float angleToTarget = Vector3.Angle(dir, dirToTarget) / 180f;
            score += wFollow * angleToTarget;

            // 2️⃣ LoS (Anticipation de la visibilité)
            Vector3 predictedPos = pos + dir * 2.0f; // On teste un peu en avant
            Vector3 dirFromPredictedToTarget = (targetPos - predictedPos).normalized;
            if (Physics.Raycast(predictedPos, dirFromPredictedToTarget, Vector3.Distance(predictedPos, targetPos), obstacleMask))
            {
                score += wLoS;
            }

            // 3️⃣ Turn cost (Fluidité du mouvement)
            // On pénalise les changements brusques par rapport à la direction actuelle
            score += wTurn * (Vector3.Angle(currentDirection, dir) / 180f);

            // 4️⃣ Safety (Proximité des obstacles restants)
            // On utilise un SphereCast court pour "sentir" les murs proches
            if (Physics.SphereCast(pos, clearanceRadius * 1.5f, dir, out RaycastHit hit, lookAheadBase, obstacleMask))
            {
                // Plus l'obstacle est proche, plus le score augmente
                score += wSafe * (1f - (hit.distance / lookAheadBase));
            }

            Scores[idx] = score;

            if (score < bestScore)
            {
                bestScore = score;
                bestDirection = dir;
            }
        }
    }


    // ---------------- B4 ----------------
    void ApplyMovement()
    {
        if (bestDirection == Vector3.zero)
        {
            Debug.LogWarning("[Drone] ❌ bestDirection nulle → pas de mouvement");
            return;
        }

        if (currentDirection == Vector3.zero)
            currentDirection = bestDirection;

        float angle = Vector3.Angle(currentDirection, bestDirection) / 180f;
        if (angle < hysteresisThreshold)
            bestDirection = currentDirection;

        currentDirection = Vector3.Lerp(
            currentDirection,
            bestDirection,
            1f - Mathf.Exp(-directionSmoothing * Time.deltaTime)
        ).normalized;

        Vector3 delta = currentDirection * moveSpeed * Time.deltaTime;
        transform.position += delta;

        if (verboseLogs)
            Debug.Log($"[Drone] B4: Move {delta.magnitude:F3} dir={currentDirection}");

        if (currentDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(currentDirection),
                6f * Time.deltaTime
            );
        }

        fallbackTimer = 0f;
    }

    // ---------------- B5 ----------------
    void HandleFallback(bool useFallback)
    {
        if (useFallback)
        {
            fallbackTimer += Time.deltaTime;

            if (fallbackTimer < fallbackPauseTime)
                return;

            Debug.LogWarning("[Drone] ⚠ FALLBACK ACTIF");

            moveSpeed *= 0.6f;
            clearanceRadius *= 1.2f;

            fallbackTimer = 0f;
        }
        else
            moveSpeed = baseMoveSpeed; 
        }

    // ---------------- DEBUG DRAW ----------------
    void OnDrawGizmos()
    {
        Vector3 pos = transform.position;

        Gizmos.color = Color.yellow;
        if (followAnchor)
            Gizmos.DrawLine(pos, followAnchor.position);

        for (int i = 0; i < candidateDirections.Count; i++)
        {
            float score = Scores[i];

            if (score == float.PositiveInfinity)
                Gizmos.color = Color.gray;
            else
            {
                float t = Mathf.InverseLerp(0f, maxScore, score);
                Gizmos.color = Color.Lerp(Color.green, Color.red, t);
            }

            Gizmos.DrawRay(pos, candidateDirections[i] * debugRayLength);
        }


        if (bestDirection != Vector3.zero)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(pos, bestDirection * debugRayLength * 1.5f);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pos, clearanceRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(pos, transform.forward * lookAheadBase);
    }
}
