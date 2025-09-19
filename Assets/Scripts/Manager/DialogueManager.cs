using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    private DialoguePlayer _dialoguePlayer;
    private Dictionary<string, string> storyNameToNextDialogue;

    private List<string> currDialogue = new List<string>();   //collection of dialogue lines for the current dialogue
    private int currIndex; // index of current dialogue

    // singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        storyNameToNextDialogue = new Dictionary<string, string>();
    }

    void Start() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        FindFirstObjectByType(typeof(DialoguePlayer));
    }    

    public void SetNextDialogue(string storyName, string nextString) {
        storyNameToNextDialogue[storyName] = nextString;
    }

    public string GetNextDialogue(string storyName) {
        string nextDialogue = null;
        if (storyNameToNextDialogue.ContainsKey(storyName)) {
            nextDialogue = storyNameToNextDialogue[storyName];
        } else {
            string parsedName = storyName.Replace("Story", "");
            nextDialogue = parsedName + "Story" + GameManager.Instance.ghostManager.GetStoryIndex(GameManager.Instance.orderManager.GetCurrActiveOrderName());
        }
        return nextDialogue;
    }
}
