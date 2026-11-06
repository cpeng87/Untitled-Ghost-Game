using System.Collections;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration;
    [SerializeField] private bool flashOnSave;
    [SerializeField] private bool fadeOutOnStart = true;
    [SerializeField] private float flashVisibleDuration = 1.5f; //idk arbitrary
    private Coroutine currentFade;

    [SerializeField] private GameObject blackScreen;
    private bool blackScreenWasActive;
    private bool flashingSave;

    [SerializeField] private GameObject autosaveText;

    private void Start()
    {
        autosaveText.SetActive(false);

        if (fadeOutOnStart)
        {
            currentFade = StartCoroutine(FadeOut());
        }
        else
        {
            canvasGroup.alpha = 0;
        }
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

        if (currentFade != null)
        {
            StopCoroutine(currentFade);
            currentFade = null;
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

    private void Flash()
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }

        // Restore the original state if another save interrupts the current flash.
        if (flashingSave)
        {
            blackScreen.SetActive(blackScreenWasActive);
            flashingSave = false;
        }

        blackScreenWasActive = blackScreen.activeSelf;
        flashingSave = true;
        currentFade = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        canvasGroup.gameObject.SetActive(true);
        blackScreen.SetActive(false);
        autosaveText.SetActive(true);

        yield return Fade(0, 1, false);
        yield return new WaitForSecondsRealtime(flashVisibleDuration);
        yield return Fade(1, 0, true);

        autosaveText.SetActive(false);
        blackScreen.SetActive(blackScreenWasActive);
        flashingSave = false;
        currentFade = null;
    }
}
