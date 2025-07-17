using UnityEngine;
using TMPro;
using System.Collections;

public class MinigameSuccessFail : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI successText;
    [SerializeField] private TextMeshProUGUI failText;
    private bool isComplete;

    private void Start()
    {
        successText.enabled = false;
        failText.enabled = false;
    }

    public void MinigameResult(bool result, bool chefSkip = false)
    {
        if (!isComplete)
        {
            isComplete = true;
            StartCoroutine(ShowTextAndPauseGame(result, chefSkip));
        }
    }

    private IEnumerator ShowTextAndPauseGame(bool result, bool chefSkip)
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

        GameManager.Instance.CompleteMinigame(result, chefSkip);
    }
}
