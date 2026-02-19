using UnityEngine;

public class ArcProgressTracker : MonoBehaviour
{
    [SerializeField] private GameObject emptyStar;
    [SerializeField] private GameObject filledStar;
    [SerializeField] private GameObject reaper;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadProgress();
        GameManager.Instance.ghostManager.OnArcProgressChanged += LoadProgress;
    }

    private void LoadProgress()
    {
        foreach (Transform obj in this.GetComponentsInChildren<Transform>())
        {
            if (obj != this.transform)
            {
                Destroy(obj.gameObject);
            }
        }
        int total = GameManager.Instance.ghostManager.GetArcGhosts();
        int progress = GameManager.Instance.ghostManager.GetArcProgress();
        for (int i = 0; i < total; i++)
        {
            if (i < progress)
            {
                Instantiate(filledStar, this.transform);
            }
            else
            {
                Instantiate(emptyStar, this.transform);
            }
        }
        Instantiate(reaper, this.transform);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null && GameManager.Instance.ghostManager != null)
        {
            GameManager.Instance.ghostManager.OnArcProgressChanged -= LoadProgress;
        }
    }
}
