using UnityEngine;
using UnityEngine.UI;

public class CremeBrulee : MonoBehaviour
{

    public Slider progressSlider;

    //Increases the value of the slider by 0.1 every time a fire particle impacts the Creme Brulee
    private void OnParticleCollision(GameObject other)
    {
        progressSlider.value += 0.1f;

        //If the Creme Brulee is struck when the slider is a specific interval, trigger the game success
        if (progressSlider.value >= 85 && progressSlider.value <= 95)
        {
            GameSuccess();
        }
    }

    
    private void GameSuccess()
    {
        Debug.Log("You Win!");
    }


}
