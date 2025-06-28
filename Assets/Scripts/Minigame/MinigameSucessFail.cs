using UnityEngine;
using TMPro;
using System.Collections;

public class MinigameSuccessFail : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI successText;
    [SerializeField] private TextMeshProUGUI failText;

    private void Start()
    {
        successText.enabled = false;
        failText.enabled = false;
    }

    public void MinigameResult(bool result)
    {
        StartCoroutine(ShowTextAndPauseGame(result));
    }

    private IEnumerator ShowTextAndPauseGame(bool result)
    {
        if (result)
        {
            successText.enabled = true;
        }
        else
        {
            failText.enabled = true;
        }

        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(1);

        Time.timeScale = 1;

        GameManager.Instance.CompleteMinigame(result);
    }
}
