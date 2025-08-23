using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SoupManager : MinigameCompletion
{
    //control variables
    // [SerializeField] private float timeLeft;
    // [SerializeField] private float progress;

    [SerializeField] private int numChops;

    //ui components
    // [SerializeField] private Image progressBar;

    // [SerializeField] private TMP_Text timer;

    public int numPartOnBoard;
    [SerializeField] private Slider mixProgressBar;
    private float mixProgress;
    // [SerializeField] private MinigameSuccessFail minigameResult;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mixProgress = 0;
        numChops = 0;
    }

    // Update is called once per frame
    // void Update()
    // {
    //     timeLeft -= Time.deltaTime;
    //     timer.text = timer.text = "Time Remaining: " + Mathf.FloorToInt(timeLeft).ToString();

    //     SetProgress();
    //     if (isComplete == false)
    //     {
    //         if (timeLeft <= 0)
    //         {
    //             // Time.timeScale = 0f;
    //             Debug.Log("you lost :(");
    //             GameManager.Instance.CompleteMinigame(false);
    //             isComplete = true;
    //         }
    //         // else if (mixProgress >= 100)
    //         // {
    //         //     // Time.timeScale = 0f;
    //         //     Debug.Log("you win :D");
    //         //     GameManager.Instance.CompleteMinigame(true);
    //         //     isComplete = true;
    //         // }
    //     }
    // }

    public void CheckCompleteSoup()
    {
        if (mixProgress >= 95)
        {
            // Time.timeScale = 0f;
            minigameResult.MinigameResult(true);
        }
        // else
        // {
        //     // Time.timeScale = 0f;
        //     isComplete = true;
        //     minigameResult.MinigameResult(false);
        // }
    }

    private void SetProgress()
    {
        mixProgressBar.value = Mathf.Clamp01(mixProgress / 100);
    }

    public void AddToMixProgress(float p)
    {
        mixProgress += p;
        SetProgress();
        CheckCompleteSoup();
    }

    public void AddChop()
    {
        numChops++;
        if (numChops == 9)
        {
            FindObjectOfType<Spoon>().GetComponent<Spoon>().enabled = true;
        }
    }

    public int GetNumPartOnBoard()
    {
        return numPartOnBoard;
    }

    public void AddNumPartOnBoard(int val)
    {
        numPartOnBoard += val;
    }
    public void SubtractNumPartOnBoard(int val)
    {
        numPartOnBoard -= val;
    }
}
