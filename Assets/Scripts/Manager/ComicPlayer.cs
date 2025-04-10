using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable] 
public class Panel
{
    public List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    public Image panelImage;
}
public class ComicPlayer : MonoBehaviour
{
    public List<Panel> panels = new List<Panel>();
    private int index = 0;
    [SerializeField] private float typewriterSpeed;
    [SerializeField] private float fadeTime = 0.4f;

    private void Start()
    {
        foreach (Panel panel in panels)
        {
            foreach(TextMeshProUGUI text in panel.texts)
            {
                text.enabled = false;
            }
            if (panel.panelImage != null)
            {
                panel.panelImage.enabled = false;
            }
        }

        AdvanceComic();
    }

    public void AdvanceComic()
    {
        if (panels.Count == index)
        {
            StartCoroutine(CompleteComic());
            return;
        }
        Panel panel = panels[index];

        TypewriterEffect(panel.texts);
        if (panel.panelImage != null)
        {
            panel.panelImage.enabled = true;
            FadeIn(panel.panelImage);
        }
        index += 1;
    }

    private void FadeIn(Image image)
    {
        StartCoroutine(FadeInCoroutine(image));
    }

    private IEnumerator FadeInCoroutine(Image image)
    {
        float elapsedTime = 0f;
        Color color = image.color;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeTime);
            image.color = color;
            yield return null;
        }

        color.a = 1f;
        image.color = color;
    }

    private void TypewriterEffect(List<TextMeshProUGUI> texts)
    {
        StartCoroutine(TypewriterEffectCoroutine(texts));
    }

    private IEnumerator TypewriterEffectCoroutine(List<TextMeshProUGUI> texts)
    {

        foreach (TextMeshProUGUI text in texts)
        {
            text.enabled = true;
            text.ForceMeshUpdate();
            text.maxVisibleCharacters = 0;
            int totalCharacters = text.textInfo.characterCount;
            for (int i = 0; i < totalCharacters; i++)
            {
                text.maxVisibleCharacters = i + 1;
                yield return new WaitForSeconds(typewriterSpeed);
            }

            text.maxVisibleCharacters = totalCharacters;
        }
    }

    private IEnumerator CompleteComic()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainStorefront");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
