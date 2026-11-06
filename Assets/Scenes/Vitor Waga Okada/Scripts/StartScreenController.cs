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
    [SerializeField] private GameObject DeleteSaveButton;
    [SerializeField] private GameObject DeleteSaveConfirmationPopup;

    [SerializeField] private TextMeshProUGUI startGameText;

    private void Start()
    {
        // OptionsScreen.SetActive(false);
        AudioManager.Instance.PlaySong("Title");
        DeleteSaveConfirmationPopup.SetActive(false);
        RefreshSaveUI();
    }

    private void RefreshSaveUI()
    {
        if (SaveManager.HasSaveFile())
        {
            startGameText.text = "Load Game";
            DeleteSaveButton.SetActive(true);
        }
        else
        {
            startGameText.text = "Start Game";
            DeleteSaveButton.SetActive(false);
        }
    }

    public void DeleteSave()
    {
        AudioManager.Instance.PlaySound("ButtonDown");
        TitleScreen.SetActive(false);
        OptionsScreen.SetActive(false);
        DeleteSaveConfirmationPopup.SetActive(true);
    }

    public void ConfirmDeleteSave()
    {
        AudioManager.Instance.PlaySound("ButtonDown");
        GameManager.Instance.DeleteSave();
        DeleteSaveConfirmationPopup.SetActive(false);
        TitleScreen.SetActive(true);
        RefreshSaveUI();
    }

    public void CancelDeleteSave()
    {
        AudioManager.Instance.PlaySound("ButtonUp");
        DeleteSaveConfirmationPopup.SetActive(false);
        TitleScreen.SetActive(true);
        RefreshSaveUI();
    }

    public void startGame()
    {
        TitleScreen.SetActive(false);
        OptionsScreen.SetActive(false);
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
        AudioManager.Instance.SaveAudioPreferences();
        TitleScreen.SetActive(true);
        OptionsScreen.SetActive(false);
    }

    public void closeGame() {
        AudioManager.Instance.PlaySound("ButtonDown");
        Application.Quit();
    }
}
