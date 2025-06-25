using UnityEngine;
using System.Collections.Generic;

public class MuffinController : MonoBehaviour
{

    private bool result;
    [SerializeField] private List<FillTracker> fillTrackers = new List<FillTracker>();
    [SerializeField] private float marginOfError;

    public void checkCompletion()
    {
        if (MuffinsComplete())
        {
            Debug.Log("All muffins are complete!");
            GameManager.Instance.CompleteMinigame(true);
        }
        else
        {
            GameManager.Instance.CompleteMinigame(false);
        }
    }

    private void Update()
    {
        float sum = 1f;
        foreach (FillTracker fillTracker in fillTrackers)
        {
            sum += fillTracker.currentFillLevel;
        }
        foreach (FillTracker fillTracker in fillTrackers)
        {
            fillTracker.GetProgressBar().value = fillTracker.currentFillLevel / sum;
        }
    }

    bool MuffinsComplete() 
    {
        foreach (FillTracker fillTracker in fillTrackers) 
        {
            if (fillTracker.GetProgressBar().value < 0.33 - marginOfError || fillTracker.GetProgressBar().value > 0.33 + marginOfError) 
            {
                return false;
            }
        }
        return true;
    }
}
