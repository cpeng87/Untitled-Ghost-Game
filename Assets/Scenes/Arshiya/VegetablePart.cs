using UnityEngine;

public class VegetablePart : DraggableObject
{
    [SerializeField] private float inc;
    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "pot") {
            Debug.Log("in pot");
            FindObjectOfType<SoupManager>().AddToProgress(inc);
            FindObjectOfType<SoupManager>().AddChop();
            gameObject.SetActive(false);
            
        }
    }
}
