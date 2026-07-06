using System.Collections;
using System.Collections.Generic;
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
        // OptionsScreen.SetActive(false);
        AudioManager.Instance.PlaySong("Title");
    }

    public void startGame()
    {
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame() {
        AudioManager.Instance.PlaySound("ButtonDown");
        yield return GameManager.Instance.SwitchToSceneCoroutine(StartGameAfterScreen);
    }

    public void goToOptions() {
        AudioManager.Instance.PlaySound("ButtonDown");
        TitleScreen.SetActive(false);
        OptionsScreen.SetActive(true);
        // print("Going to Options");
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
