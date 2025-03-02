using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    private float timer = 20f;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            SceneManager.LoadScene("BakingCake");
        }
    }
}
