using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour
{
    [SerializeField] 
    public GameObject TitleScreen;
    public GameObject OptionsScreen;
    public string StartGameAfterScreen;

    private void Start()
    {
        AudioManager.Instance.PlaySong("Title");
    }

    public void startGame() {
        AudioManager.Instance.PlaySound("ButtonDown");
        SceneManager.LoadScene(StartGameAfterScreen);
    }

    public void goToOptions() {
        AudioManager.Instance.PlaySound("ButtonDown");
        TitleScreen.SetActive(false);
        OptionsScreen.SetActive(true);
        print("Going to Options");
    }

    public void goToTitle() {
        AudioManager.Instance.PlaySound("ButtonUp");
        TitleScreen.SetActive(true);
        OptionsScreen.SetActive(false);
    }

    public void closeGame() {
        AudioManager.Instance.PlaySound("ButtonDown");
        Application.Quit();
    }
}
