using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera frontCamera;
    [SerializeField] private Camera middleCamera;
    [SerializeField] private Camera backCamera;

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
    }
    public void SwapToMiddleCamera()
    {
        frontCamera.gameObject.SetActive(false);
        middleCamera.gameObject.SetActive(true);
        backCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
    }
    public void SwapToBackCamera()
    {
        frontCamera.gameObject.SetActive(false);
        middleCamera.gameObject.SetActive(false);
        backCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
    }
    public void SwapToMainCamera()
    {
        frontCamera.gameObject.SetActive(false);
        middleCamera.gameObject.SetActive(false);
        backCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }
}
