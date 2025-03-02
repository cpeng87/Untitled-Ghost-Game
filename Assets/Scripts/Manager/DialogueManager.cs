using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    //ui elements
    // [SerializeField] private GameObject panel;
    // [SerializeField] private TMP_Text nameField;
    // [SerializeField] private TMP_Text dialogueField;
    private DialoguePlayer _dialoguePlayer;
    private Dictionary<string, string> ghostNameToNextDialogue;

    private List<string> currDialogue = new List<string>();   //collection of dialogue lines for the current dialogue
    private int currIndex; // index of current dialogue
    private bool completeOrder = false;  //flag for if the ghost needs to be deleted at the end of the dialogue
    private int seatNum = -1;   //keeps track of current dialogue ghost's seat number

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
        ghostNameToNextDialogue = new Dictionary<string, string>();
    }

    void Start() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        FindFirstObjectByType(typeof(DialoguePlayer));
    }    

    //hides the ui elements, and switches state to main
    // Currently called in the Dialogue Manager prefab, under Dialogue Runner OnDialogueComplete()


    // public void CompleteOrderDialogue(string ghostName, int seatNum, bool res) {
    //     CameraManager.Instance.SwapToSeatCamera(seatNum);
    //     this.seatNum = seatNum;
    //     if (res) {
    //         dialoguePlayer.
    //     } else {
    //         dialogueRunner.StartDialogue("Failure");
    //     }
    // }

    // Function called to queue up the order dialogue
    // public void StartOrderDialogue(string ghostName, string recipe, int seatNum) {
    //     CameraManager.Instance.SwapToSeatCamera(seatNum);
    //     dialogueRunner.StartDialogue("Order");
    // }

    public void SetNextDialogue(string ghostName, string nextString) {
        // ghostNameToNextDialogue.Add(ghostName, nextString);
        // if (string..IsNullOrEmpty(ghostNameToNextDialogue[ghostName])) {
        //     ghostNameToNextDialogue.Add(ghostName, nextString);
        // }
        ghostNameToNextDialogue[ghostName] = nextString;
        Debug.Log(ghostName + " " + nextString);
    }

    public string GetNextDialogue(string ghostName) {
        string nextDialogue = null;
        Debug.Log("Grabbing the next dialogue for " + ghostName);
        if (ghostNameToNextDialogue.ContainsKey(ghostName)) {
            nextDialogue = ghostNameToNextDialogue[ghostName];
        } else {
            nextDialogue = ghostName + "Story";
        }
        Debug.Log("Next dialogue is " + nextDialogue);
        return nextDialogue;
    }

    //starts dialogue by activating dialogue ui and swaps to the correct seat number camera
    // public void StartDialogue(string name, int seatNum, bool res)
    // {
    //     // panel.SetActive(true);
    //     // nameField.text = name;
    //     // dialogueField.text = dialogue[0];
    //     // currDialogue = dialogue;
    //     // currIndex = 0;
    //     // GameManager.Instance.SwitchToDialogue();
    //     // this.seatNum = seatNum;
    //     // Debug.Log("Seat number of dialogue: " + seatNum);
    //     CheckDialogueRunner();
        
    //     CameraManager.Instance.SwapToSeatCamera(seatNum); // Setting the appropriate Seat Number Camera here for now due to it being the most convenient
    //     // currentGhostObject = GhostSpawningManager.Instance.GetSpawnedGhost(seatNum);
    //     // dialogueRunner.StartDialogue(GetNextDialogue(name));
    //     if (res) {
    //         dialogueRunner.StartDialogue("Success");
    //     } else {
    //         dialogueRunner.StartDialogue("Failure");
    //     }
    //     // dialogueRunner.StartDialogue("Failure");
    //     // dialogueRunner.StartDialogue("Success");e
    // }


    // I DON'T WANT TO SEE THIS

    // //parses each ghost's dialogue from the .txt file and sets the lines in the scriptable ghost object
    // private void Start()
    // {
    //     // if (GameManager.Instance.parsedDialogue == false)
    //     // {
    //         // panel.SetActive(false);
    //     //     foreach (Ghost ghost in GameManager.Instance.ghostManager.GetGhostScriptables())
    //     //     {
    //     //         (List<string> success, List<string> failure, List<string> order, List<List<string>> story) = ParseDialogue(ghost.dialogue);
    //     //         ghost.success = success;
    //     //         ghost.failure = failure;
    //     //         ghost.story = story;
    //     //         ghost.order = order;
    //     //     }
    //     //     GameManager.Instance.parsedDialogue = true;
    //     // }
    // }

        //advance dialogue when clicked down and in dialogue state
    // private void Update()
    // {
    //     // if (GameManager.Instance.state == State.Dialogue)
    //     // {
    //     //     if (Input.GetKeyDown(KeyCode.Space))
    //     //     {
    //     //         AdvanceDialogue();
    //     //     }
    //     //     /*if (Input.GetMouseButtonDown(0))
    //     //     {
    //     //         AdvanceDialogue();
    //     //     }*/
    //     // }
    // }

    //starts dialogue and flags that the ghost should be deleted once the dialogue is complete, set seatnum so correct ghost is deleted
    // public void CompleteOrderDialogue(string name,  int seatNum, bool res)
    // {
    //     // StartDialogue(name, seatNum, res);
    //     //delete ghost signal
    //     // completeOrder = true;
    //     // this.seatNum = seatNum;

    // }

    //returns in the format of success, failure, and then a list of story dialogues in a tuple
    // public (List<string>, List<string>, List<string>, List<List<string>>) ParseDialogue(TextAsset asset)
    // {
    //     List<string> success = null;
    //     List<string> failure = null;
    //     List<string> order = null;
    //     List<List<string>> story = new List<List<string>>();

    //     List<string> currentList = null;

    //     if (asset == null)
    //     {
    //         Debug.LogError("Dialogue file is null!");
    //         return (null, null, null, null);
    //     }

    //     string[] lines = asset.text.Split('\n');

    //     foreach (string line in lines)
    //     {
    //         string trimmedLine = line.Trim();

    //         if (string.IsNullOrEmpty(trimmedLine))
    //             continue;

    //         //tag check
    //         if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
    //         {
    //             string tag = trimmedLine.Substring(1, trimmedLine.Length - 2).ToLower();

    //             if (tag == "success")
    //             {
    //                 success = new List<string>();
    //                 currentList = success;
    //             }
    //             else if (tag == "failure")
    //             {
    //                 failure = new List<string>();
    //                 currentList = failure;
    //             }
    //             else if (tag == "order")
    //             {
    //                 order = new List<string>();
    //                 currentList = order;
    //             }
    //             else if (tag == "story")
    //             {
    //                 List<string> newStorySegment = new List<string>();
    //                 story.Add(newStorySegment);
    //                 currentList = newStorySegment;
    //             }
    //             else if (tag == "end")
    //             {
    //                 currentList = null;
    //             }
    //         }
    //         else
    //         {
    //             if (currentList != null)
    //             {
    //                 currentList.Add(trimmedLine);
    //             }
    //         }
    //     }
    //     return (success, failure, order, story);
    // }

    
    //increments curring index and updates ui display. If reached end of dialogue, ends the dialogue
    // public void AdvanceDialogue()
    // {
    //     currIndex += 1;

    //     // (Joseph 1 / 18 / 25) Potential implementation is just have a check here for any emotions and if so, play that then advance again
    //     // Do through animator for now, change implementation if needed later
    //     if (currIndex >= currDialogue.Count)
    //     {
    //         EndDialogue();
    //     }
    //     else
    //     {
    //         if (currDialogue[currIndex].Contains("{play}")) {
    //             PlayAnimation(currDialogue[currIndex].Substring(6).Trim());
    //             AdvanceDialogue();
    //         } else {
    //             dialogueField.text = currDialogue[currIndex];
    //         }

    //     }
    // }

}
