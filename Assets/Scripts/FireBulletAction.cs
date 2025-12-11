using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireBulletAction : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float fireSpeed = 20f;

    private void Start()
    {
        var grabbable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);
    }

    // ---------------------------------------------------------
    //  Tir + utilisation de la pool BulletPool
    //  Les FX viennent de FXAddressables.Instance
    // ---------------------------------------------------------
    public void FireBullet(ActivateEventArgs arg)
    {
        // --- Récupération d'une balle depuis la pool ---
        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = spawnPoint.position;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = spawnPoint.forward * fireSpeed;

        StartCoroutine(ReturnAfterDelay(bullet, 5f));

        // --- FX Muzzle (chargé depuis FXAddressables) ---
        GameObject muzzlePrefab = FXAddressables.Instance.muzzleFX;

        if (muzzlePrefab != null)
        {
            GameObject fx = Instantiate(muzzlePrefab, spawnPoint.position, spawnPoint.rotation);
            Destroy(fx, 0.5f);
        }
        else
        {
            Debug.LogWarning("⚠ Muzzle FX pas encore chargé ou introuvable !");
        }
    }

    private IEnumerator ReturnAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        BulletPool.Instance.ReturnBullet(bullet);
    }
}
