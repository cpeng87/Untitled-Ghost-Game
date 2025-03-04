using UnityEngine;
using UnityEngine.UI;

public class FillTracker : MonoBehaviour
{
    private Slider progressBar;
    public bool isFull = false;
    public float currentFillLevel = 0f;

    [SerializeField] GameObject progressBarPrefab;
    [SerializeField] Transform canvasTransform; 

    void Start()
    {
        if (progressBarPrefab != null && canvasTransform != null)
        {
            GameObject progressBarInstance = Instantiate(progressBarPrefab, canvasTransform);
            progressBar = progressBarInstance.GetComponent<Slider>();

            // Position the progress bar on top of the cup (and centered)
            Vector3 progressBarPosition = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
            progressBarInstance.transform.position = Camera.main.WorldToScreenPoint(progressBarPosition + Vector3.up * 2);
        }
    }

    public Slider GetProgressBar() {
        return progressBar;
    }
}
