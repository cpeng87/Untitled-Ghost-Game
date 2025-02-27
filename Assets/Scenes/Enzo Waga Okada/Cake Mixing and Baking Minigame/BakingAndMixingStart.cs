using UnityEngine;
using UnityEngine.SceneManagement;

public class BakingAndMixingStart : MonoBehaviour
{
    public void startMinigame()
    {
        SceneManager.LoadSceneAsync("CakeMixing");
    }
}
