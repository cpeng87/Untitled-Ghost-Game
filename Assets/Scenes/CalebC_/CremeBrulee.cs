using UnityEngine;
using UnityEngine.UI;

public class CremeBrulee : MonoBehaviour
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
    }

    
    // private void GameSuccess()
    // {
    //     Debug.Log("You Win!");
    //     //GameManager.Instance.CompleteMinigame(true);
    // }

    public void CheckResults()
    {
        bool result = progressSlider.value >= 85;
        if(result)
        {
            Debug.Log("success");
        }
        else
        {
            Debug.Log("fail");
        }
        GameManager.Instance.CompleteMinigame(result);
    }

    


}
