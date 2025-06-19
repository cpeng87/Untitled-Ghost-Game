using UnityEngine;

public class VegetablePart : Clickable
{
    [SerializeField] private float inc;
    
    protected override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    OnClicked();
                }
            }
        }
    }
    
    protected override void OnClicked()
    {
        SoupManager soupManager = FindObjectOfType<SoupManager>();
        // soupManager.AddToProgress(inc);
        soupManager.AddChop();
        soupManager.SubtractNumPartOnBoard(1);
        gameObject.SetActive(false);
    }
}


// using UnityEngine;

// public class VegetablePart : DraggableObject
// {
//     [SerializeField] private float inc;
//     public void OnTriggerEnter(Collider other) {
//         if (other.gameObject.name == "pot") {
//             // Debug.Log("in pot");
//             FindObjectOfType<SoupManager>().AddToProgress(inc);
//             FindObjectOfType<SoupManager>().AddChop();
//             gameObject.SetActive(false);
            
//         }
//     }
// }
