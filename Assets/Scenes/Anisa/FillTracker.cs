using UnityEngine;
using UnityEngine.UI;

public class FillTracker : MonoBehaviour
{
    private Slider progressBar;
    public bool isFull = false;
    public float currentFillLevel = 0f;
    public float numParticles;

    [SerializeField] GameObject progressBarPrefab;
    [SerializeField] Transform canvasTransform; 

    void Start()
    {
        if (progressBarPrefab != null && canvasTransform != null)
        {
            GameObject progressBarInstance = Instantiate(progressBarPrefab, canvasTransform);
            progressBar = progressBarInstance.GetComponent<Slider>();

            progressBarInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            // Position the progress bar on top of the cup (and centered)
            Vector3 progressBarPosition = new Vector3(transform.position.x - 0.4f, transform.position.y, transform.position.z);
            progressBarInstance.transform.position = Camera.main.WorldToScreenPoint(progressBarPosition + Vector3.up * 2);
        }
    }

    public Slider GetProgressBar() {
        return progressBar;
    }
}
