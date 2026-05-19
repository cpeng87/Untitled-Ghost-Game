using UnityEngine;

public class BobaCleaner : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        Destroy(collider.gameObject);
    }
}
