using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanelHandler : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        //This layers a pause to prevent weird Timescale stuff when showing tutorial hints
        PauseManager.AddPause(this.gameObject);
        PauseManager.SetPauseState(true);
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        //This removes a pause to prevent weird Timescale stuff when showing tutorial hints
        PauseManager.RemovePause(this.gameObject);
        PauseManager.SetPauseState(false);
    }
    public void ExitToDesktop()
    {
        Application.Quit();
    }
}