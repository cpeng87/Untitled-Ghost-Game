using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanelHandler : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;

    private void Start()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        AudioManager.Instance.PlaySound("ButtonDown");
        pauseMenu.SetActive(true);
        //This layers a pause to prevent weird Timescale stuff when showing tutorial hints

        PauseManager.Instance.PauseGame();
    }
    public void Resume()
    {
        AudioManager.Instance.PlaySound("ButtonUp");
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        //This removes a pause to prevent weird Timescale stuff when showing tutorial hints
        PauseManager.Instance.UnpauseGame();
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
            GameManager.Instance.Reset();
        }
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Reset();
        }

        PauseManager.Instance.UnpauseGame();
        SceneManager.LoadScene("TitleScene");
    }

    public void OpenOptions()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
}