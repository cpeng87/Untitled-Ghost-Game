using UnityEngine;
using Yarn.Unity;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(DialogueRunner))]
public class DialoguePlayer : MonoBehaviour
{
    public static DialoguePlayer Instance { get; private set; }
    [SerializeField] private DialogueRunner dialogueRunner;

    private string currentOrder;
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

    void Start() {
        dialogueRunner.AddCommandHandler<string, string>("play", PlayAnimation);
        dialogueRunner.AddCommandHandler<string>("setCam", SetCamera);
        dialogueRunner.AddCommandHandler<string>("startStory", StartStoryDialogue);
        dialogueRunner.AddCommandHandler<string, string>("setNext", SetNextDialogue);
        dialogueRunner.AddCommandHandler("end", EndDialogue);
    }

    public static void PlayAnimation(string name, string animation) {
        Debug.Log("Searching for ghost by then name" + name);
        GameObject target = GhostSpawningManager.Instance.GetSpawnedGhost(name);
        if (target == null) Debug.Log("Target is null");
        target.GetComponent<Animator>().Play(animation);
    }

    [YarnFunction("GetOrder")]
    public static string GetOrder() {
        // int selectedIndex = (int) (Random.value * 3);
        // string[] names = {"tea", "coffee", "milk"};
        // return names[selectedIndex];
        return DialoguePlayer.Instance.currentOrder;
    }

    // This function physically hurts me to write
    // Also outdated with other camera changes
    public static void SetCamera(string cameraName) {
        Transform cameras = CameraManager.Instance.transform;
        Transform camToSwitchTo = cameras.Find(cameraName);
        if (camToSwitchTo) {
            for (int i = 0; i < cameras.childCount; i++) {
                Transform cam = cameras.GetChild(i); 
                if (!cam.name.Equals(cameraName)) {
                    cam.gameObject.SetActive(false);
                }
            }
            camToSwitchTo.gameObject.SetActive(true);
        }
    }

    public void StartStoryDialogue(string ghostName) {
        StartCoroutine(StoryDialogue(ghostName));
    }

    private IEnumerator StoryDialogue(string ghostName) {
        yield return new WaitForSeconds(0.01f);
        dialogueRunner.StartDialogue(DialogueManager.Instance.GetNextDialogue(ghostName));
    }

    public void SetNextDialogue(string name, string dialogueNode) {
        DialogueManager.Instance.SetNextDialogue(name, dialogueNode);
    }

    public void EndDialogue()
    {
        CameraManager.Instance.SwapToMainCamera();
        Debug.Log("SeatNumber is " + seatNum);
        GhostSpawningManager.Instance.DeleteSpawnedGhost(seatNum);
        GameManager.Instance.orderManager.RemoveCompletedOrder();
    }

    // Specific Order Dialogue
    // Function called to queue up the order dialogue
    public void StartOrderDialogue(string ghostName, string recipe, int seatNum) {
        this.seatNum = seatNum;
        this.currentOrder = recipe; 
        CameraManager.Instance.SwapToSeatCamera(seatNum);
        dialogueRunner.StartDialogue(ghostName + "Order");
    }

    public void CompleteOrderDialogue(string ghostName, int seatNum, bool res) {
        CameraManager.Instance.SwapToSeatCamera(seatNum);
        this.seatNum = seatNum;
        if (res) {
            dialogueRunner.StartDialogue(ghostName + "Success");
        } else {
            dialogueRunner.StartDialogue(ghostName + "Failure");
        }
    }


}
