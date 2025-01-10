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
    public List<Recipe> recipes = new List<Recipe>();
    public State state;
    public int maxGhosts;

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

    // public void SpawnGhost()
    // {
    //     if (ghostManager.IsActiveFull() == true)
    //     {
    //         return;
    //     }
    //     List<Ghost> possibleGhost = new List<Ghost>();
    //     foreach (Recipe recipe in unlockedRecipes)
    //     {
    //         possibleGhost.AddRange(ghostManager.GetGhostsFromRecipe(recipe));
    //     }
    //     //randomize a index to check spawn, if alr has active order, keep reroll, until no active order or 100 rerolls
    //     int index = (int) (Random.value * possibleGhost.Count);
    //     int count = 0;
    //     while (ghostManager.CheckGhostIsActive(possibleGhost[index]) == true)
    //     {
    //         if (count > 100)
    //         {
    //             Debug.Log("Cannot spawn any ghost, maxed rolls");
    //             return;
    //         }
    //         index = (int) (Random.value * possibleGhost.Count);
    //         count++;
    //     }

    //     ghostManager.AddActiveGhost(possibleGhost[index]);

    // }
}
