using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject particulePC;
    public GameObject particuleMQ;
    private GameObject particule;

    void Start()
    {
#if UNITY_ANDROID
            particule = particuleMQ;
#else
        particule = particulePC;

#endif
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            // --- Explosion (PAS dans la pool)
            GameObject explosion = Instantiate(particule);
            explosion.transform.position = transform.position;
            Destroy(explosion, 0.75f);

            // --- D�sactivation de la cible (pool)
            gameObject.SetActive(false);

            // --- D�sactivation de la balle (pool)
            other.gameObject.SetActive(false);
        }
    }
}
