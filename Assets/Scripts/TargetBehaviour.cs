using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    private GameObject explosionPrefab;

    private void Start()
    {
        explosionPrefab = FXAddressables.Instance.explosionFX;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet")) return;

        GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(fx, 0.75f);

        gameObject.SetActive(false);
        other.gameObject.SetActive(false);
    }
}
