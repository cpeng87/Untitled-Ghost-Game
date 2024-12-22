using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum State
{
    Main,
    Dialogue
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public OrderManager orderManager;
    public GhostManager ghostManager;

    public List<string> minigames = new List<string>();
    private int currency;
    public List<Recipe> unlockedRecipes = new List<Recipe>();
    // public List<Ghost> ghosts = new List<Ghost>();
    public State state;

    public bool parsedDialogue = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        state = State.Main;
        orderManager = GetComponent<OrderManager>();
        ghostManager = GetComponent<GhostManager>();
        ghostManager.Setup();
    }

    public void SwitchToMinigame(string minigameName)
    {
        SceneManager.LoadScene(minigameName);
    }

    public void CompleteMinigame(bool isSuccess)
    {
        StartCoroutine(LoadSceneAndCompleteOrder("Storefront", isSuccess));
    }

    private IEnumerator LoadSceneAndCompleteOrder(string sceneName, bool isSuccess)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        orderManager.CompleteOrder(isSuccess);
        state = State.Dialogue;
    }

    public void SwitchToMain()
    {
        state = State.Main;
    }

    public void SwitchToDialogue()
    {
        state = State.Dialogue;
    }

    public void AddCurrency(int added)
    {
        currency += added;
        UIManager.Instance.UpdateCurrency(currency);
        Debug.Log("Currency: " + currency);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);

        if (state == State.Main)
        {
            Debug.Log("Main state loaded. Initialize UI elements here.");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
