using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float refreshInterval = 0.15f;

    private void OnEnable()
    {
        StartCoroutine(RefreshLoop());
    }

    private IEnumerator RefreshLoop()
    {
        while (true)
        {
            if (GameplayManager.Instance != null)
                scoreText.text = GameplayManager.Instance.GetScore().ToString();

            yield return new WaitForSeconds(refreshInterval);
        }
    }
}
