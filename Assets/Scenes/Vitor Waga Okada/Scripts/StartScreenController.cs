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
        AudioManager.Instance.PlaySong("Title");
    }

    public void startGame()
    {
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame() {
        AudioManager.Instance.PlaySound("ButtonDown");

        LoadingScreen loadingScreen = FindAnyObjectByType<LoadingScreen>();
        if (loadingScreen != null)
        {
            yield return loadingScreen.FadeIn();
        }

        // Load the new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(StartGameAfterScreen);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // SceneManager.LoadSceneAsync(StartGameAfterScreen);
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
