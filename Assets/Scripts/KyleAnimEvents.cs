using System.Collections.Generic;
using UnityEngine;

public class KyleAnimEvents : MonoBehaviour
{
    [Header("Settings")]
    public float speedFactor = 1f;
    [Range(2, 30)]
    public int sampleCount = 10; // Plus c'est haut, plus c'est fluide (mais plus lent à réagir)

    [Header("References")]
    public Animator animator;

    // Privé
    private Vector3 lastPos;
    private Queue<float> speedSamples = new Queue<float>();
    private float speedSum = 0f;

    void Start()
    {
        lastPos = transform.position;
    }

    // On utilise FixedUpdate plutôt qu'une Coroutine pour être synchro avec la physique
    void FixedUpdate()
    {
        CalculateMovingAverageSpeed();
    }

    void CalculateMovingAverageSpeed()
    {
        // 1. Calcul de la vitesse brute de cette frame
        float distance = Vector3.Distance(transform.position, lastPos);
        float rawSpeed = distance / Time.fixedDeltaTime;
        lastPos = transform.position;

        // 2. Gestion de la moyenne glissante
        speedSum += rawSpeed;
        speedSamples.Enqueue(rawSpeed);

        if (speedSamples.Count > sampleCount)
        {
            speedSum -= speedSamples.Dequeue();
        }

        // 3. Calcul de la moyenne finale
        float averageSpeed = speedSum / speedSamples.Count;

        // 4. Envoi à l'animator
        if (animator != null)
        {
            animator.SetFloat("Speed", averageSpeed * speedFactor);
        }
    }

    // Pour le debug dans l'inspecteur (optionnel)
    public float CurrentVisualSpeed => (speedSamples.Count > 0) ? (speedSum / speedSamples.Count) : 0f;

    void OnFootstep() { }
}