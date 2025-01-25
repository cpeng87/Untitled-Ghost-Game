using UnityEngine;
using UnityEngine.SceneManagement;

public class SandwichStart : Clickable
{
    protected override void OnClicked()
    {
        SceneManager.LoadScene("SandwichGame");
    }
}
