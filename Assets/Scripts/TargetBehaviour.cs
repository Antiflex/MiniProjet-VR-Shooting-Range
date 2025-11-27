using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    public GameObject particule;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            // --- Explosion (PAS dans la pool)
            GameObject explosion = Instantiate(particule);
            explosion.transform.position = transform.position;
            Destroy(explosion, 0.75f);

            // --- Désactivation de la cible (pool)
            gameObject.SetActive(false);

            // --- Désactivation de la balle (pool)
            other.gameObject.SetActive(false);
        }
    }
}
