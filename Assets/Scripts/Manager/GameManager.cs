using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum State
{
    Main,
    Dialogue,
    Recipe
}

//manages state of game
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public OrderManager orderManager;
    public GhostManager ghostManager;

    public List<string> minigames = new List<string>();
    private int currency;
    private int satisfactionLevel;
    public List<Recipe> unlockedRecipes = new List<Recipe>();
    public List<Recipe> recipes = new List<Recipe>();
    public State state;
    public int maxGhosts;
    public Arc arc;

    public bool parsedDialogue = false;

    //singleton
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
        state = State.Main;
        arc = Arc.Beginning;
        orderManager = GetComponent<OrderManager>();
        ghostManager = GetComponent<GhostManager>();
        ghostManager.Setup();
    }

    public void AddUnlockedRecipe(Recipe recipe)
    {
        if (unlockedRecipes.Contains(recipe))
        {
            Debug.Log("Already have that recipe");
        }
        else
        {
            unlockedRecipes.Add(recipe);
        }
    }

    public void SwitchToMinigame(string minigameName)
    {
        SceneManager.LoadScene(minigameName);
    }

    public void CompleteMinigame(bool isSuccess)
    {
        StartCoroutine(LoadSceneAndCompleteOrder("MainStorefront", isSuccess));
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

    public void IncreaseSatisfaction()
    {
        ++satisfactionLevel;
        UIManager.Instance.UpdateSatisfaction(satisfactionLevel);
    }

    public void DecreaseSatisfaction()
    {
        --satisfactionLevel;
        UIManager.Instance.UpdateSatisfaction(satisfactionLevel);
    }

    public int GetCurrency()
    {
        return currency;
    }

    public int GetSatisfactionLevel()
    {
        return satisfactionLevel;
    }

    public void UpdateArc()
    {
        arc = (Arc) ((int) arc + 1);

        if (arc == Arc.None)
        {
            //play ending scene you beat the game!!
        }
    }
}
