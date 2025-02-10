using UnityEngine;

public class IngredientCollide : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.attachedRigidbody != null)
        {
            MiniGame bc = c.attachedRigidbody.gameObject.GetComponent<MiniGame>();
            bc.CheckAllIngredientsConnectedTrue();
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.attachedRigidbody != null)
        {
            MiniGame bc = c.attachedRigidbody.gameObject.GetComponent<MiniGame>();
            bc.CheckAllIngredientsConnectedFalse();
        }
    }
}
