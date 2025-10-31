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

    // public List<string> minigames = new List<string>();
    private int currency;
    private int satisfactionLevel;
    public List<Recipe> unlockedRecipes = new List<Recipe>();
    public Dictionary<string, bool> hasOpenedTutorial = new Dictionary<string, bool>();
    // public List<Recipe> recipes = new List<Recipe>();
    public State state;
    public int maxGhosts;
    public Arc arc;
    public MCGhostController mcGhostController;

    public bool parsedDialogue = false;
    public Vector3 MCStartPosition;
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
        SetupTutorialDictionary();
        Debug.Log("GameManager instance ID: " + GetInstanceID());
    }

    private void SetupTutorialDictionary()
    {
        foreach (Recipe recipe in unlockedRecipes)
        {
            hasOpenedTutorial.Add(recipe.minigame, false);
        }
    }

    public void SetTutorialState(bool state)
    {
        string recipe = SceneManager.GetActiveScene().name;
        if (hasOpenedTutorial.ContainsKey(recipe))
        {
            hasOpenedTutorial[recipe] = state;
        }
    }

    public bool GetTutorialState()
    {
        string recipe = SceneManager.GetActiveScene().name;
        if (hasOpenedTutorial.ContainsKey(recipe))
        {
            return hasOpenedTutorial[recipe];
        }
        return false;
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

    public void CompleteMinigame(bool isSuccess, bool chefSkip = false, bool specialCookie = false)
    {
        state = State.Dialogue;
        StartCoroutine(CompleteMinigameCoroutine("MainStorefront", isSuccess, chefSkip, specialCookie));
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

    private IEnumerator CompleteMinigameCoroutine(string sceneName, bool isSuccess, bool chefSkip, bool specialCookie)
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
        orderManager.CompleteOrder(isSuccess, chefSkip, specialCookie);
        Time.timeScale = 1;

        // Optionally fade out the loading screen after loading
        if (loadingScreen != null)
        {
            loadingScreen.FadeOut();
        }
    }

    // private void UpdateRecipes()
    // {
    //     foreach (Recipe recipe in recipes)
    //     {
    //         if (recipe.unlockArc == arc)
    //         {
    //             unlockedRecipes.Add(recipe);
    //         }
    //     }
    // }


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
        if (mcGhostController != null)
        {
            mcGhostController.UnlockMovement();
        }
    }

    public void SwitchToDialogue()
    {
        state = State.Dialogue;
        if (mcGhostController != null)
        {
            mcGhostController.TeleportTo(MCStartPosition, Quaternion.identity);
            mcGhostController.LockMovement();
            Debug.Log("Switched to Dialogue locking movement");
        }
    }

    public void AddCurrency(int added)
    {
        currency += added;
        UIManager.Instance.UpdateCurrency(currency);
    }

    public void UpdateCurrency()
    {
        UIManager.Instance.UpdateCurrency(currency);
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
        if (arc == Arc.Legacy)
        {
            StartCoroutine(LoadEndScene());
            return;
        }

        arc = (Arc) ((int) arc + 1);

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

    public void SaveGame()
    {
        SaveData saveData = new SaveData()
        {
            state = state,
            arc = arc,
            ghostNameToStoryIndex = ghostManager.ghostNameToStoryIndex
        };
        SaveManager.SaveGame(saveData);
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
