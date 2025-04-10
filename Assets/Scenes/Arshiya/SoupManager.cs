using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SoupManager : MonoBehaviour
{
    //control variables
    [SerializeField] private float timeLeft;
    [SerializeField] private float progress;

    [SerializeField] private int numChops;

    //ui components
    [SerializeField] private Image progressBar;

    [SerializeField] private TMP_Text timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       progress = 0; 
       numChops = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        timer.text = timer.text = "Time Remaining: " + Mathf.FloorToInt(timeLeft).ToString();

        SetProgress();

        if (timeLeft <= 0) {
            Time.timeScale = 0f;
            Debug.Log("you lost :(");
        } else if (progress >= 100) {
            Time.timeScale = 0f;
            Debug.Log("you win :D");
        }
    }

    private void SetProgress()
    {
        progressBar.fillAmount = Mathf.Clamp01(progress / 100);
    }

    public void AddToProgress(float p)  {
        progress += p;
        Debug.Log("current progress: " + progress);
        SetProgress();
    }

    public void AddChop() {
        numChops++;
        if (numChops == 9) {
            FindObjectOfType<Spoon>().GetComponent<Spoon>().enabled = true;
        }
    }
}
