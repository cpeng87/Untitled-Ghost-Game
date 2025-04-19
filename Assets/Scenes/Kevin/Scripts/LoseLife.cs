using UnityEngine;

public class LoseLife : MonoBehaviour
{
    [SerializeField] private GameObject MiniGame; // Reference to the MiniGame GameObject

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collission detected");
        // Get the MiniGame component attached to the MiniGame GameObject
        MiniGame miniGameController = MiniGame.GetComponent<MiniGame>();

        if (miniGameController != null)
        {
            // Call the RemoveLife method
            // miniGameController.RemoveLife();
            Debug.Log("Call fail");
            miniGameController.Fail();
        }
    }
}
