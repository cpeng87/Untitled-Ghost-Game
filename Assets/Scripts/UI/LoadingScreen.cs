using System.Collections;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration;
    private Coroutine currentFade;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeIn()
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }
        if (!canvasGroup.gameObject.activeSelf)
        {
            canvasGroup.gameObject.SetActive(true);
        }
        yield return Fade(0, 1, false);
    }

    public IEnumerator FadeOut()
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }
        yield return new WaitForSecondsRealtime(0.1f);
        yield return Fade(1, 0, true);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, bool deactivateAfterFade)
    {
        float elapsedTime = 0;

        // Set the initial alpha
        canvasGroup.alpha = startAlpha;

        while (elapsedTime <= fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            float dt = Mathf.Min(Time.unscaledDeltaTime, 0.1f);
            elapsedTime += dt;
            // elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        if (deactivateAfterFade && endAlpha == 0)
        {
            canvasGroup.gameObject.SetActive(false);
        }
    }
}
