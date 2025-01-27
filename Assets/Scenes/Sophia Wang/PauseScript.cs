using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript1 : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void ExitToDesktop()
    {
        Application.Quit();
    }
}
