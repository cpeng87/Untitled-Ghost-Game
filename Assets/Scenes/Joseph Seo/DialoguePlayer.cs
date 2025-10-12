using UnityEngine;
using Yarn.Unity;
using System.Collections;
using System.Collections.Generic;

public enum DialogueState
{
    None,
    Success,
    Fail,
    Story
}

[RequireComponent(typeof(DialogueRunner))]
public class DialoguePlayer : MonoBehaviour
{
    public static DialoguePlayer Instance { get; private set; }
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private FancyDialogue fd;

    private string currentOrder;
    private int storyNum;
    private int seatNum = -1;

    private DialogueState state = DialogueState.None;

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

    void Start() {
        dialogueRunner.AddCommandHandler<string, string>("play", PlayAnimation);
        dialogueRunner.AddCommandHandler<string>("setCam", SetCamera);
        dialogueRunner.AddCommandHandler<string>("startStory", StartStoryDialogue);
        dialogueRunner.AddCommandHandler<string, string>("setNext", SetNextDialogue);
        dialogueRunner.AddCommandHandler("end", EndDialogue);
        // dialogueRunner.AddCommandHandler("reset", Reset);
        dialogueRunner.AddCommandHandler<bool>("reaperPitch", ReaperPitch);
        dialogueRunner.AddCommandHandler("emotionChange", (string[] args) => EmotionChange(args));

        // dialogueRunner.onDialogueComplete += OnDialogueComplete;
        dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);

    }

    public static void PlayAnimation(string name, string animation) {
        Debug.Log("Searching for ghost by then name" + name);
        GameObject target = GhostSpawningManager.Instance.GetSpawnedGhost(name);
        if (target == null) Debug.Log("Target is null");
        target.GetComponent<Animator>().Play(animation);
    }

    [YarnFunction("GetOrder")]
    public static string GetOrder() {
        return DialoguePlayer.Instance.currentOrder;
    }

    public void EmotionChange(params string[] args)
    {
        int seatNum = GameManager.Instance.orderManager.GetCurrActiveOrderSeatNum();
        GameObject ghost = GhostSpawningManager.Instance.GetSpawnedGhost(seatNum);
        if (args.Length > 1)
        {
            ghost.GetComponent<GhostObj>().EmotionChange(args[0], args[1]);
        }
        else
        {
            ghost.GetComponent<GhostObj>().EmotionChange(args[0]);
        }
    }

    public void Reset() {
        fd.Reset();
    }

    public void ReaperPitch(bool val)
    {
        AudioManager.Instance.SetReaperPitch(val);
    }

    // This function physically hurts me to write
    // Also outdated with other camera changes
    public static void SetCamera(string cameraName)
    {
        Transform cameras = CameraManager.Instance.transform;
        Transform camToSwitchTo = cameras.Find(cameraName);
        if (camToSwitchTo)
        {
            for (int i = 0; i < cameras.childCount; i++)
            {
                Transform cam = cameras.GetChild(i);
                if (!cam.name.Equals(cameraName))
                {
                    cam.gameObject.SetActive(false);
                }
            }
            camToSwitchTo.gameObject.SetActive(true);
        }
    }

    public void StartStoryDialogue(string storyToStart) {
        Debug.Log(storyToStart);
        StartCoroutine(StoryDialogue(storyToStart));
    }

    private IEnumerator StoryDialogue(string storyTitle) {
        yield return new WaitForSeconds(0.01f);
        dialogueRunner.StartDialogue(DialogueManager.Instance.GetNextDialogue(storyTitle));
    }

    public void SetNextDialogue(string name, string dialogueNode) {
        // GameManager.Instance.state = State.Dialogue;
        DialogueManager.Instance.SetNextDialogue(name, dialogueNode);
    }

    public void EndDialogue()
    {
        CameraManager.Instance.SwapToMainCamera();
        Debug.Log("SeatNumber is " + seatNum);
        GhostSpawningManager.Instance.DeleteSpawnedGhost(seatNum);
        GameManager.Instance.orderManager.RemoveCompletedOrder();
        GameManager.Instance.state = State.Main;
    }

    // Specific Order Dialogue
    // Function called to queue up the order dialogue
    public void StartOrderDialogue(string ghostName, string recipe, int seatNum) {
        this.seatNum = seatNum;
        this.currentOrder = recipe;
        CameraManager.Instance.SwapToSeatCamera(seatNum);
        if (ghostName.Contains(" Ghost"))
        {
            ghostName = ghostName.Replace(" Ghost", "");
        }
        ghostName = ghostName.Replace(" ", "");
        dialogueRunner.StartDialogue(ghostName + "Order");
        GameManager.Instance.state = State.Dialogue;
    }

    public void CompleteOrderDialogue(string ghostName, int seatNum, bool result) {
        CameraManager.Instance.SwapToSeatCamera(seatNum);
        this.seatNum = seatNum;
        string parsedName = ghostName.Replace(" Ghost", "");
        parsedName = parsedName.Replace(" ", "");

        if (GameManager.Instance.orderManager.GetCurrActiveOrderName() == "Reaper")
        {
            //change to reaper song name if changed later on
            AudioManager.Instance.PlaySong("Reaper");
        }
        
        if (result) {
            state = DialogueState.Success;
            dialogueRunner.StartDialogue(parsedName + "Success");
        } else {
            state = DialogueState.Fail;
            dialogueRunner.StartDialogue(parsedName + "Failure");
        }
    }

    private void GhostDialogueReset()
    {
        GameManager.Instance.orderManager.RemoveCompletedOrder();
        GameManager.Instance.state = State.Main;
        state = DialogueState.None;
    }

    private void OnDialogueComplete()
    {
        if (state == DialogueState.Story)
        {
            GameManager.Instance.ghostManager.IncrementStoryIndex(GameManager.Instance.orderManager.GetCurrActiveOrderName());

            //reaper special case
            if (GameManager.Instance.orderManager.GetCurrActiveOrderName() == "Reaper")
            {
                GameManager.Instance.UpdateArc();
            }
            GhostDialogueReset();
        }
        else if (state == DialogueState.Fail)
        {
            GhostDialogueReset();
        }
        else if (state == DialogueState.Success)
        {
            state = DialogueState.Story;
        }
        else
        {
            GameManager.Instance.state = State.Main;
        }

        GameManager.Instance.SaveGame();
    }
}
