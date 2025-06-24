using System.Collections;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration;

    private void Start()
    {
        FadeOut();
    }

    public Coroutine FadeIn()
    {
        if (!canvasGroup.gameObject.activeSelf)
        {
            canvasGroup.gameObject.SetActive(true);
        }
        return StartCoroutine(Fade(0, 1, deactivateAfterFade: false));
    }

    public Coroutine FadeOut()
    {
        return StartCoroutine(Fade(1, 0, deactivateAfterFade: true));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, bool deactivateAfterFade)
    {
        float elapsedTime = 0;

        // Set the initial alpha
        canvasGroup.alpha = startAlpha;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        if (deactivateAfterFade && endAlpha == 0)
        {
            canvasGroup.gameObject.SetActive(false);
        }
    }
}
