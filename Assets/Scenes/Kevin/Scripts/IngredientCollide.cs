using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class IngredientCollide : MonoBehaviour
{
    private bool hasPlayedSfx = false;
    private void OnTriggerEnter(Collider c)
    {
        Debug.Log("collide");
        AudioManager.Instance.PlaySound("Drop");
        if (c.attachedRigidbody != null && c.gameObject.CompareTag("Ingredient"))
        {
            MiniGame.Instance.RegisterIngredientCollision(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasPlayedSfx == false)
        {
            AudioManager.Instance.PlaySound("Drop");
        }
        hasPlayedSfx = true;
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.attachedRigidbody != null && c.gameObject.CompareTag("Ingredient"))
        {
            MiniGame.Instance.UnregisterIngredientCollision(this.gameObject);
        }
    }
}