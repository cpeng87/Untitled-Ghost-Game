using UnityEngine;

public class Cookbook : MonoBehaviour
{
    public static bool paused = false;
    public GameObject cookbookUI;

    private void Start()
    {
        cookbookUI.SetActive(false);
        if (GameManager.Instance.GetTutorialState() == false)
        {
            Pause();
        }
    }

    public void Resume() {
        cookbookUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        GameManager.Instance.SetTutorialState(true);
    }
    public void Pause() {
        cookbookUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }
}
