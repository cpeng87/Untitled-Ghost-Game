using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text nameField;
    [SerializeField] private TMP_Text dialogueField;

    private List<string> currDialogue = new List<string>();
    private int currIndex; // index of current dialogue
    private bool deleteGhost = false;
    private int seatNum = -1;

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

    private void Update()
    {
        if (GameManager.Instance.state == State.Dialogue)
        {
            if (Input.GetMouseButtonDown(0))
            {
                AdvanceDialogue();
            }
        }
    }

    public void CompleteOrderDialogue(string name, List<string> dialogue, int seatNum)
    {
        StartDialogue(name, dialogue, seatNum);
        //delete ghost signal
        deleteGhost = true;
        this.seatNum = seatNum;
    }

    public void StartDialogue(string name, List<string> dialogue, int seatNum)
    {
        panel.SetActive(true);
        nameField.text = name;
        dialogueField.text = dialogue[0];
        currDialogue = dialogue;
        currIndex = 0;
        GameManager.Instance.SwitchToDialogue();
        CameraManager.Instance.SwapToSeatCamera(seatNum);
    }

    //increments curring index and updates ui display. If reached end of dialogue, ends the dialogue
    public void AdvanceDialogue()
    {
        currIndex += 1;
        if (currIndex >= currDialogue.Count)
        {
            EndDialogue();
        }
        else
        {
            dialogueField.text = currDialogue[currIndex];
        }
    }

    //hides the ui elements, and switches state to main
    public void EndDialogue()
    {
        panel.SetActive(false);
        GameManager.Instance.SwitchToMain();
        currDialogue = null;
        CameraManager.Instance.SwapToMainCamera();
        if (deleteGhost)
        {
            GhostSpawningManager.Instance.DeleteSpawnedGhost(seatNum);
        }
        deleteGhost = false;
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
