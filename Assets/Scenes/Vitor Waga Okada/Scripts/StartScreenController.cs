using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartScreenController : MonoBehaviour
{
    [SerializeField] 
    public GameObject TitleScreen;
    public GameObject OptionsScreen;
    public string StartGameAfterScreen;
    public string StartGameAfterScreenWithSave;

    [SerializeField] private TextMeshProUGUI startGameText;

    private void Start()
    {
        // OptionsScreen.SetActive(false);
        AudioManager.Instance.PlaySong("Title");
        //check if there is an existing save file
        if (SaveManager.HasSaveFile())
        {
            startGameText.text = "Load Game";
        }
        else
        {
            startGameText.text = "Start Game";
        }
    }

    public void startGame()
    {
        if (SaveManager.HasSaveFile())
        {
            StartCoroutine(StartGameWithSave());
        }
        else
        {
            StartCoroutine(StartGame());
        }
    }

    public IEnumerator StartGame() {
        AudioManager.Instance.PlaySound("ButtonDown");
        yield return GameManager.Instance.SwitchToSceneCoroutine(StartGameAfterScreen);
    }

    public IEnumerator StartGameWithSave() {
        AudioManager.Instance.PlaySound("ButtonDown");
        GameManager.Instance.LoadGame();
        yield return GameManager.Instance.SwitchToSceneCoroutine(StartGameAfterScreenWithSave);
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
