using UnityEngine;
using TMPro;
using System.Collections;

public class MinigameSuccessFail : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI successText;
    [SerializeField] private TextMeshProUGUI failText;
    [SerializeField] private GameObject bg;
    private bool isComplete;

    private void Start()
    {
        successText.enabled = false;
        failText.enabled = false;
        bg.SetActive(false);
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
        Time.timeScale = 0;
        bg.SetActive(true);
        if (result)
        {
            successText.enabled = true;
            AudioManager.Instance.StopSong();
            AudioManager.Instance.PlaySound("Win");
        }
        else
        {
            failText.enabled = true;
            AudioManager.Instance.StopSong();
            AudioManager.Instance.PlaySound("Lose");
        }

        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1;

        GameManager.Instance.CompleteMinigame(result, chefSkip);
    }
}
