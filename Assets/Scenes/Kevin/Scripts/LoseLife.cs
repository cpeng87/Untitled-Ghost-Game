using UnityEngine;

public class LoseLife : MonoBehaviour
{
    [SerializeField] private GameObject MiniGame; // Reference to the MiniGame GameObject

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the MiniGame component attached to the MiniGame GameObject
        MiniGame miniGameController = MiniGame.GetComponent<MiniGame>();

        if (miniGameController != null)
        {
            // Call the RemoveLife method
            miniGameController.RemoveLife();
        }
    }
}
