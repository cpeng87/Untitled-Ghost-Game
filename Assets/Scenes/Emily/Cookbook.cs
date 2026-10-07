using UnityEngine;
using UnityEngine.EventSystems;

public class Cookbook : MonoBehaviour
{
    public static bool paused = false;
    public GameObject cookbookUI;

    private void Start()
    {
        cookbookUI.SetActive(false);
        if (GameManager.Instance != null && GameManager.Instance.GetTutorialState() == false)
        {
            Pause();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }

    public void Resume() {
        cookbookUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        EventSystem.current.SetSelectedGameObject(null);
        GameManager.Instance.SetTutorialState(true);
    }
    public void Pause() {
        cookbookUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    private void Toggle()
    {
        if (paused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
}
