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
        SceneManager.LoadScene(StartGameAfterScreen);
    }

    public void goToOptions() {
        TitleScreen.SetActive(false);
        OptionsScreen.SetActive(true);
        print("Going to Options");
    }

    public void goToTitle() {
        TitleScreen.SetActive(true);
        OptionsScreen.SetActive(false);
    }

    public void closeGame() {
        Application.Quit();
    }
}
