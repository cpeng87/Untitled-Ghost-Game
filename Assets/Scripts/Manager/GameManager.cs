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

// Manages the state of the game
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

    // Singleton
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
        StartCoroutine(SwitchToSceneCoroutine(minigameName));
    }

    public void CompleteMinigame(bool isSuccess)
    {
        StartCoroutine(CompleteMinigameCoroutine("MainStorefront", isSuccess));
    }

    private IEnumerator SwitchToSceneCoroutine(string sceneName)
    {
        // Get the loading screen
        LoadingScreen loadingScreen = FindObjectOfType<LoadingScreen>();
        if (loadingScreen != null)
        {
            // Wait for the fade-in to complete
            yield return loadingScreen.FadeIn();
        }

        // Load the new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator CompleteMinigameCoroutine(string sceneName, bool isSuccess)
    {
        // Get the loading screen
        LoadingScreen loadingScreen = FindObjectOfType<LoadingScreen>();
        if (loadingScreen != null)
        {
            // Wait for the fade-in to complete
            yield return loadingScreen.FadeIn();
        }

        // Load the new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Complete the order
        orderManager.CompleteOrder(isSuccess);
        state = State.Dialogue;

        // Optionally fade out the loading screen after loading
        if (loadingScreen != null)
        {
            loadingScreen.FadeOut();
        }
    }


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public enum State
// {
//     Main,
//     Dialogue,
//     Recipe
// }

// //manages state of game
// public class GameManager : MonoBehaviour
// {
//     public static GameManager Instance;

//     public OrderManager orderManager;
//     public GhostManager ghostManager;

//     public List<string> minigames = new List<string>();
//     private int currency;
//     private int satisfactionLevel;
//     public List<Recipe> unlockedRecipes = new List<Recipe>();
//     public List<Recipe> recipes = new List<Recipe>();
//     public State state;
//     public int maxGhosts;
//     public Arc arc;

//     public bool parsedDialogue = false;

//     //singleton
//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     void Start()
//     {
//         state = State.Main;
//         arc = Arc.Beginning;
//         orderManager = GetComponent<OrderManager>();
//         ghostManager = GetComponent<GhostManager>();
//         ghostManager.Setup();
//     }

//     public void AddUnlockedRecipe(Recipe recipe)
//     {
//         if (unlockedRecipes.Contains(recipe))
//         {
//             Debug.Log("Already have that recipe");
//         }
//         else
//         {
//             unlockedRecipes.Add(recipe);
//         }
//     }

//     public void SwitchToMinigame(string minigameName)
//     {
//         FindObjectOfType<LoadingScreen>().FadeIn();
//         SceneManager.LoadScene(minigameName);
//     }

//     public void CompleteMinigame(bool isSuccess)
//     {
//         FindObjectOfType<LoadingScreen>().FadeIn();
//         StartCoroutine(LoadSceneAndCompleteOrder("MainStorefront", isSuccess));
//     }
    
//     private IEnumerator SwitchToSceneCoroutine(string sceneName)
//     {
//         LoadingScreen loadingScreen = FindObjectOfType<LoadingScreen>();
//         if (loadingScreen != null)
//         {
//             yield return loadingScreen.FadeIn(); // Wait for FadeIn to complete
//         }
//         SceneManager.LoadScene(sceneName);
//     }

//     private IEnumerator LoadSceneAndCompleteOrder(string sceneName, bool isSuccess)
//     {
//         AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
//         while (!asyncLoad.isDone)
//         {
//             yield return null;
//         }
//         orderManager.CompleteOrder(isSuccess);
//         state = State.Dialogue;
//     }

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
        Debug.Log("Updated Currency: " + currency);
    }

    public bool SubtractCurrency(int subtracted)
    {
        currency -= subtracted;
        bool rtn = true;
        // if (currency < 0)
        // {
        //     currency = 0;
        //     rtn = false;
        // }
        if (state == State.Main)
        {
            UIManager.Instance.UpdateCurrency(currency);
        }
        Debug.Log("Updated Currency: " + currency);
        return rtn;
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
            StartCoroutine(LoadEndScene());
            return;
        }

        // TODO: Update the Tree Petal VFX based on the current arc
        switch (arc)
        {
            case Arc.Passion:
                // Set the VFX
                break;
            case Arc.Connection:
                // Set the VFX 
                break;
            case Arc.Legacy:
                // Set the VFX
                break;
            default:
                break;
        }
        ArcEvent.TriggerArcChanged();
    }


    private IEnumerator LoadEndScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("End Scene");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}
