using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class IngredientCollide : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.attachedRigidbody != null && c.gameObject.CompareTag("Ingredient"))
        {
            MiniGame.Instance.RegisterIngredientCollision(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.attachedRigidbody != null && c.gameObject.CompareTag("Ingredient"))
        {
            MiniGame.Instance.UnregisterIngredientCollision(this.gameObject);
        }
    }
}