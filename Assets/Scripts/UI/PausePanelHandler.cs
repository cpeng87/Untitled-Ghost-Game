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
        Debug.Log("Start pause");
        AudioManager.Instance.PlaySound("ButtonDown");
        pauseMenu.SetActive(true);
        Debug.Log(AudioManager.Instance);
        //This layers a pause to prevent weird Timescale stuff when showing tutorial hints

        PauseManager.AddPause(this.gameObject);
        Debug.Log(this.gameObject);
        PauseManager.SetPauseState(true);
        Debug.Log("Finished pause");
    }
    public void Resume()
    {
        AudioManager.Instance.PlaySound("ButtonUp");
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
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
        if (AudioManager.Instance != null)
        {
            Debug.Log("resetting");
            AudioManager.Instance.Reset();
        }

        PauseManager.RemovePause(this.gameObject);
        PauseManager.SetPauseState(false);
        SceneManager.LoadScene("TitleScene");
    }

    public void OpenOptions()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
}