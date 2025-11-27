using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireBulletAction : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float fireSpeed = 20;
    [SerializeField] private GameObject particule;

    private void Start()
    {
        var grabbable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);
    }

    public void FireBullet(ActivateEventArgs arg)
    {
        // --- Utilisation de la pool ---
        GameObject spawnedBullet = BulletPool.Instance.GetBullet();
        spawnedBullet.transform.position = spawnPoint.position;

        Rigidbody rb = spawnedBullet.GetComponent<Rigidbody>();
        rb.linearVelocity = spawnPoint.forward * fireSpeed;

        // Retour automatique dans la pool après 5 sec
        StartCoroutine(ReturnAfterDelay(spawnedBullet, 5f));

        // --- Explosion en Instantiate / Destroy (pas de pool) ---
        GameObject explosion = Instantiate(particule);
        explosion.transform.position = spawnPoint.position;
        Destroy(explosion, 0.5f);
    }

    private IEnumerator ReturnAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        BulletPool.Instance.ReturnBullet(bullet);
    }
}
