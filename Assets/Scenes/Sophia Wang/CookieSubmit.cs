using UnityEngine;

public class CookieSubmit : MonoBehaviour
{
    public static void onFinish()
    {
        GameManager.Instance.CompleteMinigame(true);
    }
}
