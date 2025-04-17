using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{

    public TextMeshProUGUI timerText;
    private float timeRemaining;
    public float timeLimit = 120f;
    public bool isRunning = true;

    void Start()
    {
        timeRemaining = timeLimit;
        UpdateTimerText(timeRemaining);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Max(0, timeRemaining);
            UpdateTimerText(timeRemaining);
            Debug.Log("Time left: " + timeRemaining);
        }
        
    }

    private void UpdateTimerText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
