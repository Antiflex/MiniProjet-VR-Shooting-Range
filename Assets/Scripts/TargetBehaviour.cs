using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject particule;
    [SerializeField] private int scoreValue = 10;

    private bool counted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet")) return;
        if (counted) return;

        counted = true;
        particule = FXAddressables.Instance.explosionFX;
        if (particule != null)
        {
            GameObject fx = Instantiate(particule, transform.position, Quaternion.identity);
            Destroy(fx, 0.75f);
        }

        if (GameplayManager.Instance != null)
        {
            GameplayManager.Instance.AddScore(scoreValue);
            GameplayManager.Instance.RegisterTargetDespawn();
        }

        gameObject.SetActive(false);
        other.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        counted = false;
    }
}
