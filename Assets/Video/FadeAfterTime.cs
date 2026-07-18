using System.Collections;
using UnityEngine;

public class FadeAfterTime : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float startFadeAfter = 15f;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        canvasGroup.alpha = 0f;
        StartCoroutine(StartFade());
    }

    private IEnumerator StartFade()
    {
        yield return new WaitForSeconds(startFadeAfter);

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}