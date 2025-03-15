using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class IngredientCollide : MonoBehaviour
{
    private void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null && c.gameObject.CompareTag("Ingredient"))
        {
            MiniGame.Instance.RegisterIngredientCollision(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.attachedRigidbody != null && c.gameObject.CompareTag("Ingredient"))
        {
            MiniGame.Instance.UnregisterIngredientCollision(this.gameObject);
        }
    }
}