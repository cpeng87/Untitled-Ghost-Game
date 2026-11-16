using System.Collections;
using UnityEngine;

public class Autosave : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    private bool blackScreenWasActive;
    private bool flashingSave;
    private Coroutine currentFade;
    [SerializeField] private GameObject autosaveText;
    [SerializeField] private float flashVisibleDuration = 0.5f;
    [SerializeField] private bool flashOnSave;
    [SerializeField] private float fadeDuration;

    private void Start()
    {
        canvasGroup.alpha = 0;
    }

    private void OnEnable()
    {
        if (flashOnSave)
        {
            SaveManager.SaveSucceeded += Flash;
        }
    }

    private void OnDisable()
    {
        SaveManager.SaveSucceeded -= Flash;
    }

    private void Flash()
    {
        if (flashingSave)
        {
            return;
            // blackScreen.SetActive(blackScreenWasActive);
            // flashingSave = false;
        }

        flashingSave = true;
        currentFade = StartCoroutine(FlashRoutine());
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

    private IEnumerator FlashRoutine()
    {
        canvasGroup.gameObject.SetActive(true);
        autosaveText.SetActive(true);

        yield return Fade(0, 1, false);
        yield return new WaitForSecondsRealtime(flashVisibleDuration);
        yield return Fade(1, 0, true);

        autosaveText.SetActive(false);
        flashingSave = false;
        currentFade = null;
    }
}
