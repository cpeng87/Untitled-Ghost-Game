using UnityEngine;
using UnityEngine.UI;

public class CremeBrulee : MinigameCompletion
{

    public Slider progressSlider;
    public RingScript ringScript;

    // public void Update()
    // {
    //     if(progressSlider.value >= 85)
    //     {
    //         GameSuccess();
    //     }
    // }

    public void IncreaseSlider(float value)
    {
        progressSlider.value += value;
        CheckResults();
    }

    
    // private void GameSuccess()
    // {
    //     Debug.Log("You Win!");
    //     //GameManager.Instance.CompleteMinigame(true);
    // }

    public void CheckResults()
    {
        bool result = progressSlider.value >= 100;
        if(result)
        {
            minigameResult.MinigameResult(true);
        }
    }
}
