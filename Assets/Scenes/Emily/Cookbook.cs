using UnityEngine;

public class Cookbook : MonoBehaviour
{
    public static bool paused = false;
    public GameObject cookbookUI;

    public void Resume() {
        cookbookUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }
    public void Pause() {
        cookbookUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }
}
