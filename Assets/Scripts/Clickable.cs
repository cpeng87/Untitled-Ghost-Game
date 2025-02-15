using UnityEngine;

public class Clickable : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetMouseButtonUp(0))
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

    protected virtual void OnClicked() {}
}