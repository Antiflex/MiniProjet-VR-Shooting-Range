using System.Collections;
using UnityEngine;

public class AutoDisableFX : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.5f;

    private void OnEnable()
    {
        StartCoroutine(DisableAfter());
    }

    private IEnumerator DisableAfter()
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false);
    }
}
