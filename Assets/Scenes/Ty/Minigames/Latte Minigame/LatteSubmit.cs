using UnityEngine;

public class LatteSubmit : MonoBehaviour
{
    public static void OnFinish()
    {
        Draw obj = GameObject.Find("DrawManager").GetComponent<Draw>();
        float accuracyFinal = obj.CalculateAccuracy();

        if (accuracyFinal > 0.5)
        {
            Debug.Log("pass! Accuracy: " + accuracyFinal);
            GameManager.Instance.CompleteMinigame(true);
        }
        else
        {
            Debug.Log("fail! Accuracy: " + accuracyFinal);
            GameManager.Instance.CompleteMinigame(true);
        }

        
    }
}
