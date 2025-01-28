using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    //ui elements
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text nameField;
    [SerializeField] private TMP_Text dialogueField;

    private List<string> currDialogue = new List<string>();   //collection of dialogue lines for the current dialogue
    private int currIndex; // index of current dialogue
    private bool completeOrder = false;  //flag for if the ghost needs to be deleted at the end of the dialogue
    private int seatNum = -1;   //keeps track of current dialogue ghost's seat number

    // (Joseph Seo 1 / 18 / 25) Parameters for changing expression
    // Done as GameObjects for now, but you can change it to whatever you want here
    GameObject currentGhostObject;
    // [SerializeField] private GhostManager ghostManager;

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
    }

    //parses each ghost's dialogue from the .txt file and sets the lines in the scriptable ghost object
    private void Start()
    {
        if (GameManager.Instance.parsedDialogue == false)
        {
            panel.SetActive(false);
            foreach (Ghost ghost in GameManager.Instance.ghostManager.GetGhostScriptables())
            {
                (List<string> success, List<string> failure, List<string> order, List<List<string>> story) = ParseDialogue(ghost.dialogue);
                ghost.success = success;
                ghost.failure = failure;
                ghost.story = story;
                ghost.order = order;
            }
            GameManager.Instance.parsedDialogue = true;
        }
    }

    //advance dialogue when clicked down and in dialogue state
    private void Update()
    {
        if (GameManager.Instance.state == State.Dialogue)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AdvanceDialogue();
            }
            /*if (Input.GetMouseButtonDown(0))
            {
                AdvanceDialogue();
            }*/
        }
    }

    //starts dialogue and flags that the ghost should be deleted once the dialogue is complete, set seatnum so correct ghost is deleted
    public void CompleteOrderDialogue(string name, List<string> dialogue, int seatNum)
    {
        StartDialogue(name, dialogue, seatNum);
        //delete ghost signal
        completeOrder = true;
        this.seatNum = seatNum;
    }

    //starts dialogue by activating dialogue ui and swaps to the correct seat number camera
    public void StartDialogue(string name, List<string> dialogue, int seatNum)
    {
        panel.SetActive(true);
        nameField.text = name;
        dialogueField.text = dialogue[0];
        currDialogue = dialogue;
        currIndex = 0;
        GameManager.Instance.SwitchToDialogue();
        this.seatNum = seatNum;
        Debug.Log("Seat number of dialogue: " + seatNum);
        CameraManager.Instance.SwapToSeatCamera(seatNum);
        currentGhostObject = GhostSpawningManager.Instance.GetSpawnedGhost(seatNum);
    }

    //increments curring index and updates ui display. If reached end of dialogue, ends the dialogue
    public void AdvanceDialogue()
    {
        currIndex += 1;

        // (Joseph 1 / 18 / 25) Potential implementation is just have a check here for any emotions and if so, play that then advance again
        // Do through animator for now, change implementation if needed later
        if (currIndex >= currDialogue.Count)
        {
            EndDialogue();
        }
        else
        {
            if (currDialogue[currIndex].Contains("{play}")) {
                PlayAnimation(currDialogue[currIndex].Substring(6).Trim());
                AdvanceDialogue();
            } else {
                dialogueField.text = currDialogue[currIndex];
            }

        }
    }

    private void PlayAnimation(string animation) {
        Animator ghostAnimator = currentGhostObject.GetComponentInChildren<Animator>();
        // Debug.Log(ghostAnimator.gameObject.name);
        Debug.Log(animation);
        ghostAnimator.Play(animation);
        // Debug.Log("Hit play animation!");
    }

    //hides the ui elements, and switches state to main
    public void EndDialogue()
    {
        panel.SetActive(false);
        GameManager.Instance.SwitchToMain();
        currDialogue = null;
        CameraManager.Instance.SwapToMainCamera();
        if (completeOrder)
        {
            GhostSpawningManager.Instance.DeleteSpawnedGhost(seatNum);
            GameManager.Instance.orderManager.RemoveCompletedOrder();
        }
        completeOrder = false;
    }

    //returns in the format of success, failure, and then a list of story dialogues in a tuple
    public (List<string>, List<string>, List<string>, List<List<string>>) ParseDialogue(TextAsset asset)
    {
        List<string> success = null;
        List<string> failure = null;
        List<string> order = null;
        List<List<string>> story = new List<List<string>>();

        List<string> currentList = null;

        if (asset == null)
        {
            Debug.LogError("Dialogue file is null!");
            return (null, null, null, null);
        }

        string[] lines = asset.text.Split('\n');

        foreach (string line in lines)
        {
            string trimmedLine = line.Trim();

            if (string.IsNullOrEmpty(trimmedLine))
                continue;

            //tag check
            if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
            {
                string tag = trimmedLine.Substring(1, trimmedLine.Length - 2).ToLower();

                if (tag == "success")
                {
                    success = new List<string>();
                    currentList = success;
                }
                else if (tag == "failure")
                {
                    failure = new List<string>();
                    currentList = failure;
                }
                else if (tag == "order")
                {
                    order = new List<string>();
                    currentList = order;
                }
                else if (tag == "story")
                {
                    List<string> newStorySegment = new List<string>();
                    story.Add(newStorySegment);
                    currentList = newStorySegment;
                }
                else if (tag == "end")
                {
                    currentList = null;
                }
            }
            else
            {
                if (currentList != null)
                {
                    currentList.Add(trimmedLine);
                }
            }
        }
        return (success, failure, order, story);
    }
}
