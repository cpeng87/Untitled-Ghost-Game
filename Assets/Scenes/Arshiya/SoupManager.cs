using UnityEngine;
using UnityEngine.UI;

public class SoupManager : MonoBehaviour
{
    //control variables
    [SerializeField] private static float timeLeft;
    [SerializeField] private static float progress;

    //ui components
    [SerializeField] private static Image progressBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       progress = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        SetProgress();
    }

    static private void SetProgress()
    {
        progressBar.fillAmount = Mathf.Clamp01(progress / 100);
    }

    public static void AddToProgress(float p)  {
        progress += p;
        SetProgress();
    }
}
