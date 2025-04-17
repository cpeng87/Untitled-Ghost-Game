using UnityEngine;

public class MuffinController : MonoBehaviour
{

    private bool result;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkCompletion() 
    {
        if (MuffinsComplete()) 
        {
            Debug.Log("All muffins are complete!");
            GameManager.Instance.CompleteMinigame(true);
        } else
        {
            GameManager.Instance.CompleteMinigame(false);
        }
    }

    bool MuffinsComplete() 
    {
        FillTracker[] fillTrackers = GetComponentsInChildren<FillTracker>();
        foreach (FillTracker fillTracker in fillTrackers) 
        {
            if (!fillTracker.isFull) 
            {
                return false; // If any muffin is not complete, return false
            }
        }
        return true; // All muffins are complete
    }
}
