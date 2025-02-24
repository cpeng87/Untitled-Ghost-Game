using UnityEngine;

public class CameraManagerCutscene : MonoBehaviour
{
    public static CameraManagerCutscene Instance { get; private set; }
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera frontCamera;
    [SerializeField] private Camera middleCamera;
    [SerializeField] private Camera backCamera;
    [SerializeField] private Camera mainCutsceneCamera;
    [SerializeField] private Camera NPCCutsceneCamera;
    [SerializeField] private Camera MCCutsceneCamera;

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

    void Start()
    {
        SwapToMainCamera();
    }

    public void SwapToSeatCamera(int seatNum)
    {
        if (seatNum == 0)
        {
            SwapToFrontCamera();
        }
        else if (seatNum == 1)
        {
            SwapToMiddleCamera();
        }
        else if (seatNum == 2)
        {
            SwapToBackCamera();
        }
    }

    public void SwapToFrontCamera()
    {
        frontCamera.gameObject.SetActive(true);
        middleCamera.gameObject.SetActive(false);
        backCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        mainCutsceneCamera.gameObject.SetActive(false);
        NPCCutsceneCamera.gameObject.SetActive(false);
        MCCutsceneCamera.gameObject.SetActive(false);
    }
    public void SwapToMiddleCamera()
    {
        frontCamera.gameObject.SetActive(false);
        middleCamera.gameObject.SetActive(true);
        backCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        mainCutsceneCamera.gameObject.SetActive(false);
        NPCCutsceneCamera.gameObject.SetActive(false);
        MCCutsceneCamera.gameObject.SetActive(false);
    }
    public void SwapToBackCamera()
    {
        frontCamera.gameObject.SetActive(false);
        middleCamera.gameObject.SetActive(false);
        backCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        mainCutsceneCamera.gameObject.SetActive(false);
        NPCCutsceneCamera.gameObject.SetActive(false);
        MCCutsceneCamera.gameObject.SetActive(false);
    }
    public void SwapToMainCamera()
    {
        frontCamera.gameObject.SetActive(false);
        middleCamera.gameObject.SetActive(false);
        backCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        mainCutsceneCamera.gameObject.SetActive(false);
        NPCCutsceneCamera.gameObject.SetActive(false);
        MCCutsceneCamera.gameObject.SetActive(false);
    }

    public void SwapToMainCutsceneCamera()
    {
        frontCamera.gameObject.SetActive(false);
        middleCamera.gameObject.SetActive(false);
        backCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        mainCutsceneCamera.gameObject.SetActive(true);
        NPCCutsceneCamera.gameObject.SetActive(false);
        MCCutsceneCamera.gameObject.SetActive(false);
    }

    public void SwapToNPCCutsceneCamera()
    {
        frontCamera.gameObject.SetActive(false);
        middleCamera.gameObject.SetActive(false);
        backCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        mainCutsceneCamera.gameObject.SetActive(false);
        NPCCutsceneCamera.gameObject.SetActive(true);
        MCCutsceneCamera.gameObject.SetActive(false);
    }

    public void SwapToMCCutsceneCamera()
    {
        frontCamera.gameObject.SetActive(false);
        middleCamera.gameObject.SetActive(false);
        backCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        mainCutsceneCamera.gameObject.SetActive(false);
        NPCCutsceneCamera.gameObject.SetActive(false);
        MCCutsceneCamera.gameObject.SetActive(true);
    }

}
