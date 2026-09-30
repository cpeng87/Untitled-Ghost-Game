using UnityEngine;

public class LatteSubmit : MinigameCompletion {
    [SerializeField] LatteManager latteManager;
    public void onFinish()
    {
        if (latteManager.accuracyScore >= 80f)
        {
            Debug.Log("Latte accuracy is above 80%");
            minigameResult.MinigameResult(true);
        } else
        {
            Debug.Log("Latte accuracy is below 80%");
            minigameResult.MinigameResult(false);
        }
        
    }
}