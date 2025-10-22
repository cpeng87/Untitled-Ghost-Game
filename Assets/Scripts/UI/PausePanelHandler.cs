using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanelHandler : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        AudioManager.Instance.PlaySound("ButtonDown");
        pauseMenu.SetActive(true);
        //This layers a pause to prevent weird Timescale stuff when showing tutorial hints

        PauseManager.AddPause(this.gameObject);
        PauseManager.SetPauseState(true);
    }
    public void Resume()
    {
        AudioManager.Instance.PlaySound("ButtonUp");
        pauseMenu.SetActive(false);
        //This removes a pause to prevent weird Timescale stuff when showing tutorial hints
        PauseManager.RemovePause(this.gameObject);
        PauseManager.SetPauseState(false);
    }
    public void ExitToDesktop()
    {
        AudioManager.Instance.PlaySound("ButtonDown");
        Application.Quit();
    }

    public void ExitToTitle()
    {
        AudioManager.Instance.PlaySound("ButtonDown");
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject, 1f); 
        }

        PauseManager.RemovePause(this.gameObject);
        PauseManager.SetPauseState(false);
        SceneManager.LoadScene("TitleScene");
    }
}